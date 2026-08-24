using E_Commerce.Services.AuthAPI.Models.DTOs;

namespace E_Commerce.Services.AuthAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequestDTO dto);
        Task<LoginResponseDTO> Login(LoginRequestDTO dto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
