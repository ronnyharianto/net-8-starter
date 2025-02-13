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
    /// <summary>
    /// Provides methods for managing roles in the system, including retrieving, creating, updating, and deleting roles.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    internal class RoleService(ApplicationDbContext dbContext, IMapper mapper, ILogger<RoleService> logger)
        : BaseService<RoleService>(dbContext, mapper, logger), IRoleService
    {
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync()
        {
            var dataRoles = _dbContext.Roles.AsNoTracking()
                                            .OrderBy(d => d.RoleCode)
                                            .Select(d => _mapper.Map<RoleDto>(d));

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = await dataRoles.ToListAsync()
            };
        }

        public PagingDto<RoleDto> RetrieveRolesPaging(PagingSearchInputBase input)
        {
            var retVal = new PagingDto<RoleDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var dataRoles = _dbContext.Roles.AsNoTracking()
                                            .Where(d => EF.Functions.Like(d.RoleCode, searchPattern))
                                            .OrderByDescending(d => d.Modified ?? d.Created)
                                            .Select(d => _mapper.Map<RoleDto>(d));

            retVal.ApplyPagination(input.Page, input.PageSize, dataRoles);

            return retVal;
        }

        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId)
        {
            var dataRole = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(d => d.Id == roleId);
            if (dataRole == null)
                return new("Role data is not found", ResponseCode.NotFound);

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = _mapper.Map<RoleDto>(dataRole)
            };
        }

        public async Task<BaseDto> CreateRoleAsync(RoleInput input)
        {
            var (isValid, validationMessage) = await ValidateRoleInput(input);
            if (!isValid)
                return new(validationMessage, ResponseCode.Error);

            var dataRole = _mapper.Map<Role>(input);

            await _dbContext.Roles.AddAsync(dataRole);
            await _dbContext.SaveChangesAsync();

            return new("Role data is successfully created", ResponseCode.Ok);
        }

        public async Task<BaseDto> UpdateRoleAsync(Guid roleId, RoleInput input)
        {
            var dataRole = await _dbContext.Roles.FirstOrDefaultAsync(d => d.Id == roleId);
            if (dataRole == null)
                return new("Role data is not found", ResponseCode.NotFound);

            var (isValid, validationMessage) = await ValidateRoleInput(input, roleId);
            if (!isValid)
                return new(validationMessage, ResponseCode.Error);

            _mapper.Map(input, dataRole);

            _dbContext.Roles.Update(dataRole);
            await _dbContext.SaveChangesAsync();

            return new("Role data is successfully updated", ResponseCode.Ok);
        }

        public async Task<BaseDto> DeleteRoleAsync(Guid roleId)
        {
            var dataRole = await _dbContext.Roles.FirstOrDefaultAsync(d => d.Id == roleId);
            if (dataRole == null)
                return new("Role data is not found", ResponseCode.NotFound);

            dataRole.RowStatus = 1;

            _dbContext.Roles.Update(dataRole);
            await _dbContext.SaveChangesAsync();

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
                return (false, "Role code already exists.");

            return (true, string.Empty);
        }
    }
}
