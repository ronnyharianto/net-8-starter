using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for managing permissions.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    public class PermissionService(ApplicationDbContext dbContext, IMapper mapper, ILogger<PermissionService> logger)
        : BaseService<PermissionService>(dbContext, mapper, logger)
    {
        /// <summary>
        /// Retrieves all permissions from the database.
        /// </summary>
        /// <returns>
        /// An ObjectDto containing a list of PermissionDto objects and a response code indicating the status of the operation.
        /// </returns>
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
        {
            // Query the database for all permissions without tracking changes (read-only).
            // Permissions are ordered by their PermissionCode and mapped to PermissionDto objects.
            var dataPermissions = _dbContext.Permissions.AsNoTracking()
                                                        .OrderBy(d => d.PermissionCode)
                                                        .Select(d => _mapper.Map<PermissionDto>(d));

            // Return the retrieved permissions wrapped in an ObjectDto with a response code of 'Ok'.
            return new(responseCode: ResponseCode.Ok)
            {
                Obj = await dataPermissions.ToListAsync()
            };
        }
    }
}
