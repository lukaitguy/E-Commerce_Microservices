using E_Commerce.Models.DTOs;

namespace E_Commerce.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
