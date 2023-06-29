using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Bookstore.Extension;
using Bookstore.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public ShoppingCartController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public List<CartItem> GioHang
        { 
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [HttpPost]
        [Route("/api/cart/add")]
        public IActionResult AddToCart(int bookID, int? amount)
        {
            List<CartItem> gioHang = GioHang;

            try
            {
                //Them san pham vao gio hang
                CartItem item = gioHang.SingleOrDefault(p => p.Book.BookId == bookID);

                if ( item != null)
                {
                    item.amount = item.amount + amount.Value;

                    HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                }
                else
                {
                    TbBook hh = _context.TbBooks.SingleOrDefault(p => p.BookId == bookID);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        Book = hh
                    };
                    gioHang.Add(item);
                }

                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                _notyfService.Success("Thêm sản phẩm vào giỏ hàng thành công!");
                //var sp = _context.TbBooks.Where(x => x.BookId == bookID).FirstOrDefault();
                //string a = "/" + "{" + bookID + "}" + "_" + "{" + sp.BookName + "}" + "." + "html";
                return Json(new { success = true, isVad = true}) ;
            }
            catch
            {
                return Json(new { success = false, isVad = false });
            }
        }


        [HttpPost]
        [Route("/api/cart/update")]
        public IActionResult UpdateCart(int bookID, int? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.Book.BookId == bookID);
                    if (item != null && amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("/api/cart/remove")]
        public ActionResult Remove(int bookID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.Book.BookId == bookID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }

                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Cart()
        {
            return View(GioHang);
        }
    }
}
