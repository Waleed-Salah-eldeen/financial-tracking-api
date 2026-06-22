using ExpenseTracker.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult HandleFailure<T>(Result<T> result)
        {
            switch (result.ErrorType)
            {
                case ErrorType.None:
                    return NoContent();
                case ErrorType.Validation:
                    return BadRequest(result.ErrorMessage);
                case ErrorType.NotFound:
                    return NotFound(result.ErrorMessage);
                case ErrorType.Unauthorized:
                    return Unauthorized(result.ErrorMessage);
                case ErrorType.Failure:
                    return BadRequest(result.ErrorMessage);
                default:
                    return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
