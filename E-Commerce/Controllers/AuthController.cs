using E_Commerce.Models.DTOs;
using E_Commerce.Service.IService;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequest = new();
            return View(loginRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            ResponseDto result = await _authService.LoginAsync(dto);

            if(result != null && result.Success)
            {
                LoginResponseDTO login = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(result.Result));
                await SignInUser(login);
                _tokenProvider.SetToken(login.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = result.Message;
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem {Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem {Text=SD.RoleCustomer, Value=SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
        {
            ResponseDto response = await _authService.RegisterAsync(dto);
            ResponseDto assignRole;

            if(response != null && response.Success)
            {
                if (string.IsNullOrEmpty(dto.Role)){
                    dto.Role = SD.RoleCustomer;
                }
                assignRole = await _authService.AssignRole(dto);

                if(assignRole != null && assignRole.Success)
                {
                    TempData["success"] = "Registration Successful.";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = response.Message;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem {Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem {Text=SD.RoleCustomer, Value=SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;

            return View(dto);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDTO dto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(dto.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
