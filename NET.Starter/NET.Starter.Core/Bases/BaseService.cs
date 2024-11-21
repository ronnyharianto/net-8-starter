using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess.SqlServer;

namespace NET.Starter.Core.Bases
{
    public class BaseService<T>(ApplicationDbContext dbContext, IMapper mapper, ILogger<T> logger)
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        protected readonly IMapper _mapper = mapper;
        protected readonly ILogger<T> _logger = logger;
    }
}
