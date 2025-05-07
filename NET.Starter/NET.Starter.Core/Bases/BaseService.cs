using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess.SqlServer;

namespace NET.Starter.Core.Bases
{
    /// <summary>
    /// A base service class that provides common dependencies to derived service classes.
    /// </summary>
    internal class BaseService<T>(ApplicationDbContext dbContext, IMapper mapper, ILogger<T> logger)
    {
        /// <summary>
        /// The database context for accessing the application's database.
        /// </summary>
        protected readonly ApplicationDbContext _dbContext = dbContext;

        /// <summary>
        /// The mapper service for object mapping.
        /// </summary>
        protected readonly IMapper _mapper = mapper;

        /// <summary>
        /// The logger service for capturing logs specific to the derived service.
        /// </summary>
        protected readonly ILogger<T> _logger = logger;
    }
}
