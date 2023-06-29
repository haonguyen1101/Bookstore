using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Bookstore.Models;
using Bookstore.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bookstore.Controllers
{
	public class HomeController : Controller
	{
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public HomeController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            HomeViewVM model = new HomeViewVM();

            var lsBooks = _context.TbBooks.AsNoTracking()
                .Where(x => x.Status == true && x.IsHot == true)
                .OrderByDescending(x => x.BookName)
                .ToList();

            var TinTuc = _context.TbPosts
                    .AsNoTracking()
                    .Where(x => x.Status == true && x.IsNewfeed == true)
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(3)
                    .ToList();




            model.Products = lsBooks;
            model.TinTucs = TinTuc;

            return View(model);
        }

		[Route("/lien-he.html", Name = "Contract")]
		public IActionResult Contract()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}