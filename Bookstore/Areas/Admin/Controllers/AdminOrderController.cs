using AspNetCoreHero.ToastNotification.Abstractions;
using Bookstore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using PagedList.Core;

namespace Bookstore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminOrderController : Controller
	{
		private readonly dbBookstoreContext _context;
		public INotyfService _notyfService { get; }

		public AdminOrderController(dbBookstoreContext context, INotyfService notyfService)
		{
			_context = context;
			_notyfService = notyfService;
		}

		public IActionResult OrderIndex(int page = 1, int TransID = 0)
		{
			var pageNumber = page;
			var pageSize = 10;

			List<TbOrder> lsOrders = new List<TbOrder>();

			if (TransID != 0)
			{
				lsOrders = _context.TbOrders
					.AsNoTracking()
					.Where(x => x.TransactStatusId == TransID)
					.Include(x => x.TransactStatus)
					.Include(x => x.TbOrderDetails)
					.OrderByDescending(x => x.OrderId).ToList();
			}
			else
			{
				lsOrders = _context.TbOrders
					.AsNoTracking()
					.Include(x => x.TransactStatus)
                    .Include(x => x.TbOrderDetails)
                    .OrderByDescending(x => x.OrderId).ToList();
			}

			PagedList<TbOrder> models = new PagedList<TbOrder>(lsOrders.AsQueryable(), pageNumber, pageSize);
			ViewBag.CurrentTransID = TransID;
			ViewBag.CurrentPage = pageNumber;
			ViewData["TrangThai"] = new SelectList(_context.TbTransactStatuses, "TransactStatusId", "Desription", TransID);
			return View(models);
		}

        public IActionResult Filtter(int TransID = 0)
        {
            var url = $"/Admin/AdminOrder/OrderIndex?TransID={TransID}";
            if (TransID == 0)
            {
                url = $"/Admin/AdminOrder/OrderIndex";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.TbOrders
                .Include(o => o.Customer)
                .Include(o => o.TransactStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var Chitietdonhang = _context.TbOrderDetails
                .Include(x => x.Book)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.OrderDetailId)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;
            return View(order);
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.TbOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["TrangThai"] = new SelectList(_context.TbTransactStatuses, "TransactStatusId", "Desription", order.TransactStatusId);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int madondat, int trangthaiid)
        {


            var orderid = _context.TbOrders.Where(x => x.OrderId == madondat).FirstOrDefault();
			if (orderid != null)
			{
				orderid.TransactStatusId = trangthaiid;
				_context.Update(orderid);
				await _context.SaveChangesAsync();
			}
			else
				return NotFound();
			return RedirectToAction(nameof(OrderIndex));
        }

        private bool TbOrderExists(int id)
        {
            return (_context.TbBooks?.Any(e => e.BookId == id)).GetValueOrDefault();
        }

    }
}
