using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Services.ProductService;

namespace MvcEcomerce.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _service;

        public ShopController(IProductService service)
        {
            _service = service;
        }
        public IActionResult Index(int? category)
        {
            var result = _service.GetProducts(category);

            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var result = _service.SearchProducts(query);

            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var result = _service.GetProduct(id);

            if(result == null)
            {
                TempData["Message"] = "Not found product";
                return Redirect("/404");
            }

            return View(result);
        }
    }
}
