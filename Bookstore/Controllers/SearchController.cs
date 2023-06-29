using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Bookstore.Helpper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Controllers
{
    public class SearchController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public SearchController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<TbBook> ls = new List<TbBook>();

            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                ls = _context.TbBooks
                    .AsNoTracking()
                    .Include(x => x.Type)
                    .OrderByDescending(x => x.BookId).ToList();
                return PartialView("ListBooksSearchPatial", ls);
            }

            ls = _context.TbBooks.AsNoTracking()
                                  .Include(a => a.Type)
                                  .Where(x => x.BookName.Contains(keyword))
                                  .OrderByDescending(x => x.BookName)
                                  .Take(10)
                                  .ToList();

            if (ls == null)
            {
                return PartialView("ListBooksSearchPatial", null);
            }
            else
            {
                return PartialView("ListBooksSearchPatial", ls);
            }
        }

    }
}
