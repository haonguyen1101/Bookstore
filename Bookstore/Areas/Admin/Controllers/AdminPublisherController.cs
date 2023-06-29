using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Entity;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPublisherController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminPublisherController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult PublisherIndex(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;
            var lsPublisher = _context.TbPublishers.AsNoTracking().OrderByDescending(x => x.PublisherName);

            PagedList<TbPublisher> models = new PagedList<TbPublisher>(lsPublisher, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        public IActionResult PublisherCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublisherCreate([Bind("PublisherId,PublisherName,Status")] TbPublisher tbPublisher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbPublisher);
                await _context.SaveChangesAsync();
                _notyfService.Success("Đã thêm nhà xuất bản");
                return RedirectToAction(nameof(PublisherIndex));
            }
            return View(tbPublisher);
        }

        public async Task<IActionResult> PublisherEdit(int? id)
        {
            if (id == null || _context.TbPublishers == null)
            {
                return NotFound();
            }

            var tbPublisher = await _context.TbPublishers.FindAsync(id);
            if (tbPublisher == null)
            {
                return NotFound();
            }
            return View(tbPublisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublisherEdit(int id, [Bind("PublisherId,PublisherName,Status")] TbPublisher tbPublisher)
        {
            if (id != tbPublisher.PublisherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbPublisher);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Đã cập nhật nhà xuất bản");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbPublisherExists(tbPublisher.PublisherId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PublisherIndex));
            }
            return View(tbPublisher);
        }

        private bool TbPublisherExists(int id)
        {
          return _context.TbPublishers.Any(e => e.PublisherId == id);
        }
    }
}
