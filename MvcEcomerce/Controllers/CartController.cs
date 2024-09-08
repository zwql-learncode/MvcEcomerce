using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Application.SessionExtensions;
namespace MvcEcomerce.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            _context = context;
        }

        const string CART_KEY = "MYCART";
        public List<CartItemDTO> Cart => HttpContext.Session.Get<List<CartItemDTO>>(CART_KEY) ?? new List<CartItemDTO>();
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var cartItem = Cart;
            var existingItem = cartItem.SingleOrDefault(p => p.Id == id);

            if (existingItem == null)
            {
                var product = _context.Products.SingleOrDefault(p => p.MaHh == id);

                if(product == null)
                {
                    TempData["Message"] = "Not found product";
                    return Redirect("/404");
                }

                var newItem = new CartItemDTO
                {
                    Id = product.MaHh,
                    Title = product.TenHh,
                    Price = product.DonGia ?? 0,
                    ImageUrl = product.Hinh,
                    Quantity = quantity
                };
                cartItem.Add(newItem);
            }
            else
            {
                existingItem.Quantity += quantity;
            }

            HttpContext.Session.Set(CART_KEY, cartItem);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCartItem(int id)
        {
            var cartItem = Cart;
            var item = cartItem.SingleOrDefault(p => p.Id == id);

            if (item == null)
            {
                TempData["Message"] = "Not found product";
                return Redirect("/404");
            }
            cartItem.Remove(item);
            HttpContext.Session.Set(CART_KEY, cartItem);

            return RedirectToAction("Index");
        }
    }
}
