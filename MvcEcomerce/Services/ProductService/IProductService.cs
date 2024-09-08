using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Domain.Entities;

namespace MvcEcomerce.Services.ProductService
{
    public interface IProductService
    {
        public List<ProductResponseDTO> GetProducts(int? categoryId);
        public List<ProductResponseDTO> SearchProducts(string? query);
        public ProductDetailResponseDTO GetProduct(int id);
    }
}
