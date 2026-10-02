using E_Commerce.Models.DTOs;

namespace E_Commerce.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(string product);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDTO productDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDTO productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
