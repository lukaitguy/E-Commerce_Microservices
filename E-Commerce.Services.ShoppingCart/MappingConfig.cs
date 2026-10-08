using AutoMapper;
using E_Commerce.Services.ShoppingCart.Models;
using E_Commerce.Services.ShoppingCart.Models.DTOs;

namespace E_Commerce.Services.ShoppingCart
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
            CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
        }
    }
}

