using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult TestAction() => Ok("Test message");
        



    }

}
