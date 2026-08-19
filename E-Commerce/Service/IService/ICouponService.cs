using E_Commerce.Models.DTOs;

namespace E_Commerce.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDTO couponDto);
        Task<ResponseDto?> UpdateCouponAsync(CouponDTO couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
