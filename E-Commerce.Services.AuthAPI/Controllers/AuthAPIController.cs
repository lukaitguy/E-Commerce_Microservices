using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    public class AuthAPIController : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
