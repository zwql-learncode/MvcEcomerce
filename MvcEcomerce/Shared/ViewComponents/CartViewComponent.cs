using Microsoft.AspNetCore.Mvc;
using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Application.SessionExtensions;

namespace MvcEcomerce.Shared.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItemDTO>>("MYCART") ?? new List<CartItemDTO>();

            var result = new CartDTO
            {
                Quantity = cart.Sum(p => p.Quantity),
                TotalAmount = cart.Sum(p => p.Amount)
            };

            return View("CartPanel" ,result);
        }
    }
}
