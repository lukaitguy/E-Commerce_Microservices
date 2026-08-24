using Azure;
using E_Commerce.Services.AuthAPI.Models.DTOs;
using E_Commerce.Services.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    public class AuthAPIController : Controller
    {
        private readonly IAuthService _authService;
        protected ResultDto _result;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _result = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _result.Success = false;
                _result.Message = errorMessage;
                return BadRequest(_result);
            }

            return Ok(_result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _authService.Login(model);
            if(loginResponse.User == null)
            {
                _result.Success = false;
                _result.Message = "Username or password is incorrect";
                return BadRequest(_result);
            }
            _result.Result = loginResponse;
            return Ok(_result);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDTO model)
        {
            var assignRole = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRole)
            {
                _result.Success = false;
                _result.Message = "Error encountered";
                return BadRequest(_result);

            }
            return Ok(_result);
        }
    }
}
