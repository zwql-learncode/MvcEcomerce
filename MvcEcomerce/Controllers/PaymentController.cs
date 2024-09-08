using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcEcomerce.Application.DTOs.RequestDTO;
using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Application.SessionExtensions;
using MvcEcomerce.Domain.Entities;

namespace MvcEcomerce.Controllers
{
    public class PaymentController : Controller
    {
        public PaymentController(DataContext context)
        {
            _context = context;
        }
        const string CART_KEY = "MYCART";
        private readonly DataContext _context;

        public List<CartItemDTO> Cart => HttpContext.Session.Get<List<CartItemDTO>>(CART_KEY) ?? new List<CartItemDTO>();

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if(Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutDTO request)
        {
            if (ModelState.IsValid) 
            {
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "CustomerID").Value;
                var order = new HoaDon
                {
                    MaKh = customerId,
                    DiaChi = request.Address,
                    GhiChu = request.Phone,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "GRAB",
                    MaTrangThai = 0
                };

                _context.Database.BeginTransaction();
                try
                {
                    _context.Database.CommitTransaction();
                    _context.Add(order);
                    _context.SaveChanges();

                    var orderItems = new List<ChiTietHd>();
                    foreach(var item in Cart)
                    {
                        var orderItem = new ChiTietHd
                        {
                            MaHd = order.MaHd,
                            SoLuong = item.Quantity,
                            DonGia = item.Price,
                            MaHh = item.Id,
                            GiamGia = 0
                        };
                        orderItems.Add(orderItem);
                    }

                    _context.AddRange(orderItems);
                    _context.SaveChanges();

                    HttpContext.Session.Set<List<CartItemDTO>>(CART_KEY, new List<CartItemDTO>());

                    return View("Success");
                }
                catch
                {
                    _context.Database.RollbackTransaction();
                };
            }

            return View(Cart);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
