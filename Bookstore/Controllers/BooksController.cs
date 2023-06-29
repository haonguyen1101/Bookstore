using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Bookstore.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PagedList.Core;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public BooksController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [Route("shop.html", Name = "ShopProducts")]
        public IActionResult ShopIndex(int page = 1, int TypeID = 0)
        {
            try
            {
                var pageNumber = page;
                var pageSize = 8;

                List<TbBook> lsBooks = new List<TbBook>();

                if (TypeID != 0)
                {
                    lsBooks = _context.TbBooks
                        .AsNoTracking()
                        .Where(x => x.TypeId == TypeID)
                        .Include(x => x.Type)
                        .OrderByDescending(x => x.BookId).ToList();
                }
                else
                {
                    lsBooks = _context.TbBooks
                        .AsNoTracking()
                        .Include(x => x.Type)
                        .OrderByDescending(x => x.BookId).ToList();
                }

                PagedList<TbBook> models = new PagedList<TbBook>(lsBooks.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentTypeID = TypeID;
                ViewBag.CurrentPage = pageNumber;
                ViewData["TheLoai"] = new SelectList(_context.TbBookTypes, "TypeId", "TypeName", TypeID);
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
        public IActionResult ListType(string TypeName, int page = 1)
        {
            var pageSize = 10;
            var type = _context.TbBookTypes.AsNoTracking().SingleOrDefault(x => x.TypeName == TypeName);
            var lsType = _context.TbBooks
                                .AsNoTracking()
                                .Where(x => x.TypeId == type.TypeId)
                                .OrderByDescending(x => x.BookName);
            PagedList<TbBook> models = new PagedList<TbBook>(lsType, page, pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CurrentType = type;
            return View(models);
        }

        public IActionResult Filtter(int TypeID = 0)
        {
            var url = $"/shop.html?TypeID={TypeID}";
            if (TypeID == 0)
            {
                url = $"/shop.html";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        [Route("/{id}_{bookname}.html", Name = "BookDetails")]
        public IActionResult BookDetails(int id)
        {
            try
            {
                var Book = _context.TbBooks.Include(x => x.Type).Include(x => x.Publisher)
                                            .FirstOrDefault(x => x.BookId == id);

                if (Book == null)
                {
                    return RedirectToAction("ShopIndex");
                }

                var lsBooks = _context.TbBooks.AsNoTracking()
                                                 .Where(x => x.TypeId == Book.TypeId && x.BookId != id && x.Status == true)
                                                 .OrderByDescending(x => x.BookId)
                                                 .Take(4)
                                                 .ToList();

                ViewBag.Book = lsBooks;
                return View(Book);
            }
            catch
            {
                return RedirectToAction("ShopIndex", "Books");
            }
        }

        [HttpPost]
        public JsonResult UpdateSL([FromForm] int productid, [FromForm] int quantity)
        {
            var sampham = _context.TbBooks.Where(x => x.BookId == productid).FirstOrDefault();
            var Book = _context.TbBooks.Include(x => x.Type).Include(x => x.Publisher)
                                            .FirstOrDefault(x => x.BookId == productid);
            var lsBooks = _context.TbBooks.AsNoTracking()
                                                 .Where(x => x.TypeId == sampham.TypeId && x.BookId != productid && x.Status == true)
                                                 .OrderByDescending(x => x.BookId)
                                                 .Take(4)
                                                 .ToList();
            ViewBag.Book = lsBooks;
            if (sampham.Quantity < quantity)
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_ViewAll", Book), sl = sampham.Quantity });
            }
            else
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", Book) });
            }
        }
    }
}
