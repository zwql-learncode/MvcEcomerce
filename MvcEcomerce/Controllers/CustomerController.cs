using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcEcomerce.Application.DataEncryptionExtensions;
using MvcEcomerce.Application.DTOs.RequestDTO;
using MvcEcomerce.Application.Helper;
using MvcEcomerce.Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MvcEcomerce.Controllers
{
    public class CustomerController : Controller
    {
        private readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDTO request)
        {
            if (ModelState.IsValid) 
            {
                var randomKey = Utilities.GenerateRamdomKey();
                var customer = new KhachHang
                {
                    MaKh = request.Id,
                    HoTen = request.FullName,
                    Email = request.Email,
                    DienThoai = request.Phone,
                    GioiTinh = request.Sex,
                    DiaChi = request.Address,
                    NgaySinh = request.DateOfBirth ?? DateTime.MinValue,
                    RandomKey = randomKey,
                    MatKhau = request.Password.ToMd5Hash(randomKey),
                    HieuLuc = true,
                    VaiTro = 0
                };

                _context.KhachHangs.Add(customer);
                _context.SaveChanges();

                return Redirect("/");
            }
            return View(request);
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl; 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (!ModelState.IsValid) 
            {
                return View(request);
            }

            var customer = _context.KhachHangs.SingleOrDefault(c => c.MaKh == request.Id);

            if (customer == null)
            {
                ModelState.AddModelError("error", "Wrong account");
                return View(request);
            }

            if (customer.MatKhau != request.Password.ToMd5Hash(customer.RandomKey))
            {
                ModelState.AddModelError("error", "Wrong password");
                return View(request);
            }

            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, customer.Email),
                                new Claim(ClaimTypes.Name, customer.HoTen),
                                new Claim("CustomerID", customer.MaKh),
								new Claim(ClaimTypes.Role, "Customer")
                            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
