using AutoMapper;
using E_Commerce.Services.ProductAPI.Models;
using E_Commerce.Services.ProductAPI.Models.DTOs;

namespace E_Commerce.Services.ProductAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
           CreateMap<ProductDTO, Product>();
           CreateMap<Product, ProductDTO>();
        }
    }
}
