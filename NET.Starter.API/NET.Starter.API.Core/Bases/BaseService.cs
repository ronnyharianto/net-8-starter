using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.API.DataAccess;

namespace NET.Starter.API.Core.Bases
{
    internal class BaseService<T>(ApplicationDbContext dbContext, IMapper mapper, ILogger<T> logger)
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        protected readonly IMapper _mapper = mapper;
        protected readonly ILogger<T> _logger = logger;
    }
}
