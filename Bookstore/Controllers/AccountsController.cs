using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Bookstore.Extension;
using Bookstore.Helpper;
using Bookstore.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bookstore.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AccountsController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.TbCustomers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (khachhang != null)
                {
                    return Json(data: "Số điện thoại: " + Phone + "Đã được sử dụng");
                }
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.TbCustomers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                {
                    return Json(data: "Email: " + Email + "Đã được sử dụng");
                }
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.TbCustomers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.TbOrders
                        .Include(x => x.TransactStatus)
                        .AsNoTracking()
                        .Where(x => x.CustomerId == khachhang.CustomerId)
                        .OrderByDescending(x => x.OrderDate).ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }    
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> Register(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    TbCustomer khachhang = new TbCustomer
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        CreateDate = DateTime.Now,
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //Lưu session
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký tài khoản thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            var adminID = HttpContext.Session.GetString("AdministrationID");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }
            if (adminID != null)
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginVM taikhoan, string returnUrl = null)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    //KT email
                    bool isEmail = Utilities.IsValidEmail(taikhoan.UserName);
                    if (!isEmail)
                        return View(taikhoan);

                //KT tài khoản
                var temp1 = _context.TbCustomers.Where(x => x.Email == taikhoan.UserName).FirstOrDefault();
                var temp2 = _context.TbAdministrations.Where(x => x.Email == taikhoan.UserName).FirstOrDefault();
                if (temp1 == null)
                {
                    if (temp2 != null)
                    {
                        string pass = (taikhoan.Password + temp2.Salt.Trim()).ToMD5();
                        if (temp2.Password != pass)
                        {
                            _notyfService.Error("Thông tin đăng nhập không chính xác!");
                            return View(taikhoan);
                        }
                        else
                        {
                            HttpContext.Session.SetString("AdministrationID", temp2.AdministrationId.ToString());
                            var taikhoanID = HttpContext.Session.GetString("AdministrationID");
                            //Identity
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, temp2.FullName),
                                new Claim("AdministrationID", temp2.AdministrationId.ToString())
                            };
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(claimsPrincipal);
                            _notyfService.Success("Đăng nhập thành công");
                            return RedirectToAction("Admin", "AdminHome", new { area = "Admin" });
                        }

                    }
                    else
                    {
                        _notyfService.Error("Thông tin đăng nhập không chính xác!");
                        return View(taikhoan);
                    }
                }
                else
                {
                    string pass = (taikhoan.Password + temp1.Salt.Trim()).ToMD5();
                    if (temp1.Password != pass)
                    {
                        _notyfService.Error("Thông tin đăng nhập không chính xác!");
                        return View(taikhoan);
                    }
                    else
                    {
                        HttpContext.Session.SetString("CustomerId", temp1.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, temp1.FullName),
                            new Claim("CustomerId", temp1.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng nhập thành công");
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(returnUrl);
                        } 
                    }
                }
            }
            catch
            {
                return RedirectToAction("Register", "Accounts");
            }
            return View(taikhoan);
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ls = await _context.TbOrderDetails.FirstOrDefaultAsync(m => m.BookId == id);
            var ls = _context.TbOrderDetails.AsNoTracking()
                                            .Where(s => s.OrderId == id)
                                            .Include(s => s.Book).ToList();
            if (ls == null)
            {
                return NotFound();
            }

            return View(ls);
        }

        [HttpGet]
        [Route("dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            HttpContext.Session.Remove("AdministrationID");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                if (ModelState.IsValid)
                {
                    var taikhoan = _context.TbCustomers.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null)
                    {
                        return RedirectToAction("Login", "Accounts");
                    }

                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    if (pass == taikhoan.Password)
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Thay đổi mật khẩu thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }    
            }
            catch
            {
                _notyfService.Error("Thay đổi mật khẩu không thành công");
                return RedirectToAction("Dashboard", "Accounts");
            }
            _notyfService.Error("Thay đổi mật khẩu không thành công");
            return RedirectToAction("Dashboard", "Accounts");
        }
    }
}
