using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using System.Net;

namespace NET.Starter.Core.Services.Security
{
    internal class RoleService(ApplicationDbContext _dbContext, IMapper _mapper, ILogger<RoleService> _logger) : IRoleService
    {
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync()
        {
            _logger.LogInformation("Starting to retrieve all roles.");

            var roles = _dbContext.Roles.AsNoTracking()
                                        .OrderBy(d => d.Code)
                                        .Select(d => _mapper.Map<RoleDto>(d));

            var result = await roles.ToListAsync();

            _logger.LogInformation("Successfully retrieved {Count} roles.", result.Count);
            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = result
            };
        }

        public async Task<PagingDto<RoleDto>> RetrieveRolesPagingAsync(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve roles with paging. Page: {Page}, Size: {PageSize}, SearchKey: {SearchKey}",
                input.Page, input.PageSize, input.SearchKey);

            var retVal = new PagingDto<RoleDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var roles = _dbContext.Roles.AsNoTracking()
                                        .Where(d => EF.Functions.ILike(d.Code, searchPattern))
                                        .OrderByDescending(d => d.Modified ?? d.Created)
                                        .ThenBy(d => d.Code)
                                        .Select(d => _mapper.Map<RoleDto>(d));

            await retVal.ApplyPagination(input.Page, input.PageSize, roles);

            _logger.LogInformation("Successfully retrieved {Count} roles on page {Page}.", retVal.PageRecordCount, input.Page);
            return retVal;
        }

        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId)
        {
            _logger.LogInformation("Starting to retrieve role by id: {RoleId}.", roleId);

            var role = await _dbContext.Roles.AsNoTracking()
                                             .Include(r => r.RolePermissions)
                                                .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d => d.Id == roleId);
            if (role is null)
            {
                _logger.LogError("Role data is not found for id: {RoleId}.", roleId);
                return new("Role data is not found", HttpStatusCode.NotFound);
            }

            _logger.LogInformation("Successfully retrieved role by id: {RoleId}.", roleId);
            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = _mapper.Map<RoleDto>(role)
            };
        }

        public async Task<BaseDto> CreateRoleAsync(RoleInput input)
        {
            _logger.LogInformation("Starting to create role.");

            var (isValid, validationMessage) = await ValidateRoleInput(input);
            if (!isValid)
            {
                _logger.LogWarning("Role creation validation failed: {ValidationMessage}", validationMessage);
                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var role = _mapper.Map<Role>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created role: {@Role}", role);
            return new("Role data is successfully created", HttpStatusCode.OK);
        }

        public async Task<BaseDto> UpdateRoleAsync(Guid roleId, RoleInput input)
        {
            _logger.LogInformation("Starting to update role by id: {RoleId}.", roleId);

            var (isValid, validationMessage) = await ValidateRoleInput(input, roleId);
            if (!isValid)
            {
                _logger.LogWarning("Role update validation failed: {ValidationMessage}", validationMessage);
                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var role = await _dbContext.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(d => d.Id == roleId);
            if (role is null)
            {
                _logger.LogWarning("Role not found for update: {RoleId}.", roleId);
                return new("Role data is not found", HttpStatusCode.NotFound);
            }

            _mapper.Map(input, role);

            // Remove permissions not present in input (soft delete)
            var deleteRolePermissions = from d in role.RolePermissions
                                        where !input.PermissionIds.Contains(d.PermissionId)
                                        select d;
            foreach (var rolePermission in deleteRolePermissions)
            {
                rolePermission.RowStatus = 1;
            }

            // Add new permissions from input
            var addRolePermissions = from i in input.PermissionIds
                                     where !role.RolePermissions.Any(d => d.PermissionId == i)
                                     select i;
            if (addRolePermissions.Any())
                await _dbContext.RolePermissions.AddRangeAsync(addRolePermissions.Select(permissionId => new RolePermission { RoleId = roleId, PermissionId = permissionId }));

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated role by id: {RoleId}.", roleId);
            return new("Role data is successfully updated", HttpStatusCode.OK);
        }

        public async Task<BaseDto> DeleteRoleAsync(Guid roleId)
        {
            _logger.LogInformation("Starting to delete role by id: {RoleId}.", roleId);

            var role = await _dbContext.Roles.FirstOrDefaultAsync(d => d.Id == roleId);
            if (role is null)
            {
                _logger.LogWarning("Role not found for deletion: {RoleId}.", roleId);
                return new("Role data is not found", HttpStatusCode.NotFound);
            }

            role.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted role by id: {RoleId}.", roleId);
            return new("Role data is successfully deleted", HttpStatusCode.OK);
        }

        private async Task<(bool isValid, string validationMessage)> ValidateRoleInput(RoleInput input, Guid? roleId = null)
        {
            if (!input.PermissionIds.Any())
                return (false, "Please add at least one permission.");

            var dataDuplicateRole = await _dbContext.Roles.FirstOrDefaultAsync(d => d.Code == input.Code && d.Id != roleId);
            if (dataDuplicateRole != null)
                return (false, "Role already exists.");

            return (true, string.Empty);
        }
    }
}
