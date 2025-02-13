using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for managing and retrieving permissions in the system.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    internal class PermissionService(ApplicationDbContext dbContext, IMapper mapper, ILogger<PermissionService> logger)
        : BaseService<PermissionService>(dbContext, mapper, logger), IPermissionService
    {
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
        {
            var dataPermissions = _dbContext.Permissions.AsNoTracking()
                                                        .OrderBy(d => d.PermissionCode)
                                                        .Select(d => _mapper.Map<PermissionDto>(d));

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = await dataPermissions.ToListAsync()
            };
        }
    }
}
