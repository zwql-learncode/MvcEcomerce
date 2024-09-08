using MvcEcomerce.Application.DTOs.ResponseDTO;

namespace MvcEcomerce.Services.CategoryService
{
    public interface ICategoryService
    {
        public List<CategoryResponseDTO> GetCategories();
    }
}
