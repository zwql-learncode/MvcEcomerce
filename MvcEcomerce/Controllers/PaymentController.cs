using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcEcomerce.Application.DTOs.RequestDTO;
using MvcEcomerce.Application.DTOs.ResponseDTO;
using MvcEcomerce.Application.PaypalClient;
using MvcEcomerce.Application.SessionExtensions;
using MvcEcomerce.Domain.Entities;

namespace MvcEcomerce.Controllers
{
    public class PaymentController : Controller
    {
        public PaymentController(DataContext context, PaypalClient paypalClient)
        {
            _context = context;
            _paypalClient = paypalClient;
        }

        private readonly DataContext _context;
        private readonly PaypalClient _paypalClient;

        const string CART_KEY = "MYCART";
        public List<CartItemDTO> Cart => HttpContext.Session.Get<List<CartItemDTO>>(CART_KEY) ?? new List<CartItemDTO>();

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }

            ViewBag.PaypalClientdId = _paypalClient.ClientId;
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
                    foreach (var item in Cart)
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

        #region Paypal

        [Authorize]
        [HttpPost("/Payment/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            var totalAmount = Cart.Sum(p => p.Amount).ToString();
            var curr = "USD";
            var invoiceCode = "OD" + DateTime.Now.Ticks.ToString();

            try
            {
                var res = await _paypalClient.CreateOrder(totalAmount, curr, invoiceCode);

                return Ok(res);
            }
            catch (Exception ex)
            {
                var err = new { ex.GetBaseException().Message };

                return BadRequest(err);
            }
        }

        [Authorize]
        [HttpPost("/Payment/capture-paypal-order")] 
        public async Task<IActionResult> CapturePaypalOrder(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _paypalClient.CaptureOrder(orderId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                var err = new { ex.GetBaseException().Message };

                return BadRequest(err);
            }
        }
        #endregion Paypal

    }
}
