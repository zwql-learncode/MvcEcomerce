using Microsoft.EntityFrameworkCore;
using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Domain.Entities;

namespace MvcEcomerce.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }
        public List<ProductResponseDTO> GetProducts(int? categoryId)
        {
            var products = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.MaLoai == categoryId.Value);
            }

            var result = products.Select(p => new ProductResponseDTO
            {
                Id = p.MaHh,
                Title = p.TenHh,
                Price = p.DonGia ?? 0,
                ImageUrl = p.Hinh,
                Description = p.MoTaDonVi,
                CategoryName = p.Category.TenLoai
            });

            return result.ToList();
        }

        public List<ProductResponseDTO> SearchProducts(string? query)
        {
            var products = _context.Products.AsQueryable();

            if (query !=null)
            {
                products = products.Where(p => p.TenHh.Contains(query));
            }

            var result = products.Select(p => new ProductResponseDTO
            {
                Id = p.MaHh,
                Title = p.TenHh,
                Price = p.DonGia ?? 0,
                ImageUrl = p.Hinh,
                Description = p.MoTaDonVi,
                CategoryName = p.Category.TenLoai
            });

            return result.ToList();
        }

        public ProductDetailResponseDTO GetProduct(int id)
        {
            var product = _context.Products
                             .Include(p => p.Category)
                             .SingleOrDefault(p => p.MaHh == id);

            var result = new ProductDetailResponseDTO
            {
                Id = product.MaHh,
                Title = product.TenHh,
                Price = product.DonGia ?? 0,
                Description = product.MoTa,
                ImageUrl = product.Hinh,
                ShortDescription = product.MoTaDonVi,
                CategoryName = product.Category.TenLoai,
                Quantity = 100,
                Point = 5,
            };

            return result;
        }
    }
}
