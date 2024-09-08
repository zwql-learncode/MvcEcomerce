using Microsoft.AspNetCore.Mvc;
using MvcEcomerce.Services.CategoryService;

namespace MvcEcomerce.Shared.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _service;

        public CategoryViewComponent(ICategoryService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke()
        {
            var result = _service.GetCategories();

            return View(result);
        }
    }
}
