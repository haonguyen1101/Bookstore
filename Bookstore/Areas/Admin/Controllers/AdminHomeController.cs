using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminHomeController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [Route("/Admin")]
        public IActionResult Admin()
        {
            int gia1 = 0;

            DateTime StartDate = DateTime.Today.AddDays(-30);
            DateTime EndDate1 = DateTime.Now;

            var hoadonban = _context.TbOrders.Where(s => s.OrderDate >= StartDate && s.OrderDate <= EndDate1).ToList();
            foreach (var element in hoadonban)
            {
                var dondat = _context.TbOrderDetails.Where(s => s.OrderId == element.OrderId).ToList();
                foreach (var element2 in dondat)
                {
                    if (element2.PromotionPrice == null)
                    {
                        int strSanPham = (int)(Convert.ToInt32(element2.Price) * element2.Amount);
                        gia1 += strSanPham;
                    }
                    else
                    {
                        int strSanPham = (int)(Convert.ToInt32(element2.PromotionPrice) * element2.Amount);
                        gia1 += strSanPham;
                    }
                }
            }
            ViewBag.gialai = gia1;

            ViewBag.CountBook = _context.TbBooks.ToList().Count();
            ViewBag.CountOrder = _context.TbOrders.ToList().Count();
            ViewBag.CountCustomer = _context.TbCustomers.ToList().Count();


            ViewBag.loai1 = _context.TbBooks.Where(s => s.TypeId == 1).Count();
            ViewBag.loai2 = _context.TbBooks.Where(s => s.TypeId == 3).Count();
            ViewBag.loai3 = _context.TbBooks.Where(s => s.TypeId == 4).Count();
            ViewBag.loai4 = _context.TbBooks.Where(s => s.TypeId == 5).Count();
            ViewBag.loai5 = _context.TbBooks.Where(s => s.TypeId == 6).Count();
            ViewBag.loai6 = _context.TbBooks.Where(s => s.TypeId == 7).Count();
            ViewBag.loai7 = _context.TbBooks.Where(s => s.TypeId == 1008).Count();
            ViewBag.loai8 = _context.TbBooks.Where(s => s.TypeId == 1009).Count();
            ViewBag.loai9 = _context.TbBooks.Where(s => s.TypeId == 1010).Count();

            return View();
        }
    }
}
