using E_Commerce.Models.DTOs;
using E_Commerce.Service.IService;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace E_Commerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", result.Message);
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
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem {Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem {Text=SD.RoleCustomer, Value=SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;

            return View(dto);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
