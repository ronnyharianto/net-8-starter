using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security
{
    internal class RoleService(ApplicationDbContext dbContext, IMapper mapper, ILogger<RoleService> logger)
        : BaseService<RoleService>(dbContext, mapper, logger), IRoleService
    {
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync()
        {
            _logger.LogInformation("Starting to retrieve all roles.");

            var roles = _dbContext.Roles.AsNoTracking()
                                        .OrderBy(d => d.RoleCode)
                                        .Select(d => _mapper.Map<RoleDto>(d));

            _logger.LogInformation("Successfully retrieved all roles.");

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = await roles.ToListAsync()
            };
        }

        public PagingDto<RoleDto> RetrieveRolesPaging(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve paging of roles.");

            var retVal = new PagingDto<RoleDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var roles = _dbContext.Roles.AsNoTracking()
                                        .Where(d => EF.Functions.Like(d.RoleCode, searchPattern))
                                        .OrderByDescending(d => d.Modified ?? d.Created)
                                        .Select(d => _mapper.Map<RoleDto>(d));

            retVal.ApplyPagination(input.Page, input.PageSize, roles);

            _logger.LogInformation("Successfully retrieved paging of roles.");

            return retVal;
        }

        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId)
        {
            _logger.LogInformation("Starting to retrieve role by id: {RoleId}.", roleId);

            var role = await _dbContext.Roles.AsNoTracking()
                                             .Include(r => r.RolePermissions)
                                                .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d => d.Id == roleId);
            if (role == null)
            {
                _logger.LogError("Role data is not found for id: {RoleId}.", roleId);

                return new("Role data is not found", ResponseCode.NotFound);
            }
                
            _logger.LogInformation("Successfully retrieved role by id: {RoleId}.", roleId);

            return new(responseCode: ResponseCode.Ok)
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
                _logger.LogError("Failed to create role. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, ResponseCode.Error);
            }

            var role = _mapper.Map<Role>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created role.");

            return new("Role data is successfully created", ResponseCode.Ok);
        }

        public async Task<BaseDto> UpdateRoleAsync(Guid roleId, RoleInput input)
        {
            _logger.LogInformation("Starting to update role by id: {RoleId}.", roleId);

            var role = await _dbContext.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(d => d.Id == roleId);
            if (role == null)
            {
                _logger.LogError("Role data is not found for id: {RoleId}.", roleId);

                return new("Role data is not found", ResponseCode.NotFound);
            }

            var (isValid, validationMessage) = await ValidateRoleInput(input, roleId);
            if (!isValid)
            {
                _logger.LogError("Failed to update role. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, ResponseCode.Error);
            }

            _mapper.Map(input, role);

            #region Delete permission does not exists on input

            var deleteRolePermissions = from d in role.RolePermissions
                                        where !input.PermissionIds.Contains(d.PermissionId)
                                        select d;

            foreach (var rolePermission in deleteRolePermissions)
            {
                rolePermission.RowStatus = 1;
            }

            #endregion

            #region Add permission does new on input

            var addRolePermissions = from i in input.PermissionIds
                                     where !role.RolePermissions.Any(d => d.PermissionId == i)
                                     select i;

            if (addRolePermissions.Any())
                await _dbContext.RolePermissions.AddRangeAsync(addRolePermissions.Select(permissionId => new RolePermission { RoleId = roleId, PermissionId = permissionId }));

            #endregion

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated role by id: {RoleId}.", roleId);

            return new("Role data is successfully updated", ResponseCode.Ok);
        }

        public async Task<BaseDto> DeleteRoleAsync(Guid roleId)
        {
            _logger.LogInformation("Starting to delete role by id: {RoleId}.", roleId);

            var role = await _dbContext.Roles.FirstOrDefaultAsync(d => d.Id == roleId);
            if (role == null)
            {
                _logger.LogError("Role data is not found for id: {RoleId}.", roleId);

                return new("Role data is not found", ResponseCode.NotFound);
            }
                
            role.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted role by id: {RoleId}.", roleId);

            return new("Role data is successfully deleted", ResponseCode.Ok);
        }

        /// <summary>
        /// Validates the role input for duplicate role codes and required permissions.
        /// </summary>
        /// <param name="input">The role input to validate.</param>
        /// <returns>
        /// A tuple containing a boolean indicating validity and a validation message.
        /// </returns>
        private async Task<(bool isValid, string validationMessage)> ValidateRoleInput(RoleInput input, Guid? roleId = null)
        {
            if (!input.PermissionIds.Any())
                return (false, "Please add at least one permission.");

            var dataDuplicateRole = await _dbContext.Roles.FirstOrDefaultAsync(d => d.RoleCode == input.RoleCode && d.Id != roleId);
            if (dataDuplicateRole != null)
                return (false, "Role already exists.");

            return (true, string.Empty);
        }
    }
}
