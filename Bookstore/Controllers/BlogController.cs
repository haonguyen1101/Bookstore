using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Bookstore.Controllers
{
    public class BlogController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public BlogController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult BlogIndex(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsTinDangs = _context.TbPosts.AsNoTracking()
                                           .OrderByDescending(x => x.CreatedDate);

            PagedList<TbPost> models = new PagedList<TbPost>(lsTinDangs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
    }
}
