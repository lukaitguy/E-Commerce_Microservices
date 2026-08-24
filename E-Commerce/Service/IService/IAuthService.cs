using E_Commerce.Models.DTOs;

namespace E_Commerce.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDto> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<ResponseDto> AssignRole(RegisterRequestDTO registerRequestDTO);
    }
}
