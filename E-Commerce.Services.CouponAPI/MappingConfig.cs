using AutoMapper;
using E_Commerce.Services.CouponAPI.Models;
using E_Commerce.Services.CouponAPI.Models.DTOs;

namespace E_Commerce.Services.CouponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
           CreateMap<CouponDTO, Coupon>();
           CreateMap<Coupon, CouponDTO>();
        }
    }
}
