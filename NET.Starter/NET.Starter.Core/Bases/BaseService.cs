using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess.SqlServer;

namespace NET.Starter.Core.Bases
{
    /// <summary>
    /// A base service class that provides common dependencies to derived service classes.
    /// </summary>
    /// <typeparam name="T">The type of the service inheriting from this base class. Used for logging purposes.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BaseService{T}"/> class.
    /// </remarks>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    public class BaseService<T>(ApplicationDbContext dbContext, IMapper mapper, ILogger<T> logger)
    {
        /// <summary>
        /// The database context for accessing the application's database.
        /// </summary>
        protected readonly ApplicationDbContext _dbContext = dbContext;

        /// <summary>
        /// The mapper service for converting between domain models and DTOs.
        /// </summary>
        protected readonly IMapper _mapper = mapper;

        /// <summary>
        /// The logger service for logging information, warnings, and errors.
        /// </summary>
        protected readonly ILogger<T> _logger = logger;
    }
}
