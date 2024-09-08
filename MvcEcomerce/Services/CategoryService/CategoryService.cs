using MvcEcomerce.Application.DTOs.ResponseDTO;

namespace MvcEcomerce.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public List<CategoryResponseDTO> GetCategories()
        {
            var result = _context.Categories.Select(category => new CategoryResponseDTO
            {
                Id = category.MaLoai,
                Title = category.TenLoai,
                Quantity = category.Products.Count
            });

            return result.ToList();
        }
    }
}
