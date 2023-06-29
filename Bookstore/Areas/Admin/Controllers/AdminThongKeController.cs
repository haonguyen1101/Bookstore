using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Bookstore.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminThongKeController : Controller
	{
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminThongKeController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult ThongKe()
		{

            var items = 
                        from ctdd in _context.TbOrderDetails 
                        join m in _context.TbBooks on ctdd.BookId equals m.BookId
                        group ctdd by new { MaSP = m.BookId, TenSP = m.BookName } into g
                        select new QuantittyProuductModel
                        {
                            MaSP = g.Key.MaSP,
                            TenSP = g.Key.TenSP,
                            SoLuong = (int)g.Sum(s => s.Amount)
                        };
            ViewBag.Quantity = items.OrderByDescending(s => s.SoLuong).Take(10);

            ViewBag.CountBook = _context.TbBooks.ToList().Count();
            ViewBag.CountOrder = _context.TbOrders.ToList().Count();
            ViewBag.CountCustomer = _context.TbCustomers.ToList().Count();


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


            ViewBag.loai1 = _context.TbBooks.Where(s => s.TypeId == 1).Count();
            ViewBag.loai2 = _context.TbBooks.Where(s => s.TypeId == 3).Count();
            ViewBag.loai3 = _context.TbBooks.Where(s => s.TypeId == 4).Count();
            ViewBag.loai4 = _context.TbBooks.Where(s => s.TypeId == 5).Count();
            ViewBag.loai5 = _context.TbBooks.Where(s => s.TypeId == 6).Count();
            ViewBag.loai6 = _context.TbBooks.Where(s => s.TypeId == 7).Count();
            ViewBag.loai7 = _context.TbBooks.Where(s => s.TypeId == 1008).Count();
            ViewBag.loai8 = _context.TbBooks.Where(s => s.TypeId == 1009).Count();
            ViewBag.loai9 = _context.TbBooks.Where(s => s.TypeId == 1010).Count();


            DateTime EndDate = DateTime.Now;
            string temp = Convert.ToString(EndDate);
            string[] listnow = temp.Split('/');
            string s = listnow[0]; //thang
            string y = listnow[1];// ngay
            string z = listnow[2];// nam+gio

            string[] listnow1 = z.Split(' ');
            string a = listnow1[0]; //nam hien tai
            ViewBag.namhientai = a;
            string b = listnow1[1];//gio
            int t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, t8 = 0, t9 = 0, t10 = 0, t11 = 0, t12 = 0;
            var hoadon = _context.TbOrders.ToList();
            foreach (var element in hoadon)
            {

                string temp1 = Convert.ToString(element.OrderDate);
                string[] listnow2 = temp1.Split('/');
                string thang = listnow2[0]; //thang
                int thang1 = Convert.ToInt32(thang);

                string ngay = listnow2[1];// ngay
                string nam = listnow2[2];// nam+gio
                string[] listnow3 = nam.Split(' ');
                string namhientai = listnow3[0]; //nam hien tai              
                if (namhientai == a)
                {
                    int mot = 1, hai = 2, ba = 3, bon = 4, nam1 = 5, sau = 6, bay = 7, tam = 8, chin = 9, muoi = 10, muoimot = 11, muoihai = 12;
                    if (thang1 == mot)
                    {
                        t1++;
                    }
                    if (thang1 == hai)
                    {
                        t2++;
                    }
                    if (thang1 == ba)
                    {
                        t3++;
                    }
                    if (thang1 == bon)
                    {
                        t4++;
                    }
                    if (thang1 == nam1)
                    {
                        t5++;
                    }
                    if (thang1 == sau)
                    {
                        t6++;
                    }
                    if (thang1 == bay)
                    {
                        t7++;
                    }
                    if (thang1 == tam)
                    {
                        t8++;
                    }
                    if (thang1 == chin)
                    {
                        t9++;
                    }
                    if (thang1 == muoi)
                    {
                        t10++;
                    }
                    if (thang1 == muoimot)
                    {
                        t11++;
                    }
                    if (thang1 == muoihai)
                    {
                        t12++;
                    }
                }
            }
            ViewBag.t1 = t1;
            ViewBag.t2 = t2;
            ViewBag.t3 = t3;
            ViewBag.t4 = t4;
            ViewBag.t5 = t5;
            ViewBag.t6 = t6;
            ViewBag.t7 = t7;
            ViewBag.t8 = t8;
            ViewBag.t9 = t9;
            ViewBag.t10 = t10;
            ViewBag.t11 = t11;
            ViewBag.t12 = t12;

            return View();
		}
	}
}
