using E_Commerce.Services.AuthAPI.Data;
using E_Commerce.Services.AuthAPI.Models;
using E_Commerce.Services.AuthAPI.Models.DTOs;
using E_Commerce.Services.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if(user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                    await _userManager.AddToRoleAsync(user, roleName);
                    return true;
                }
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO dto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == dto.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if(user == null || isValid == false)
            {
                return new LoginResponseDTO() { User = null, Token = "" };
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDto = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegisterRequestDTO dto)
        {
            ApplicationUser user = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                NormalizedEmail = dto.Email.ToUpper(),
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == dto.Email);
                    UserDTO userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {

            }
            return "Error encountered";
        }
    }
}
