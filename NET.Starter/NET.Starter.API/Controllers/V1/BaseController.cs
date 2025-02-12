using Microsoft.AspNetCore.Mvc;

namespace NET.Starter.API.Controllers.V1
{
    /// <summary>
    /// Represents a base controller that provides common functionality for all API controllers.
    /// </summary>
    [ApiController]
    public abstract class BaseController : Controller
    {
        // This class is intended to be inherited by specific controllers to share common functionality,
        // such as logging, error handling, or other reusable methods.
    }
}