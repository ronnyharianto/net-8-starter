using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess;
using NET.Starter.Shared.Objects.Dtos;
using System.Net;

namespace NET.Starter.Core.Services.Security
{
    internal class PermissionService(ApplicationDbContext _dbContext, IMapper _mapper, ILogger<PermissionService> _logger) : IPermissionService
    {
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
        {
            _logger.LogInformation("Starting to retrieve all permissions.");

            var dataPermissions = _dbContext.Permissions.AsNoTracking()
                                                        .OrderBy(d => d.Code)
                                                        .Select(d => _mapper.Map<PermissionDto>(d));

            var result = await dataPermissions.ToListAsync();

            _logger.LogInformation("Successfully retrieved {Count} permissions.", result.Count);
            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = result
            };
        }
    }
}
