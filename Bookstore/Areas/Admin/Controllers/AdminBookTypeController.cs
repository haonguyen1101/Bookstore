using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Entity;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using PagedList.Core;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBookTypeController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminBookTypeController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult BookTypeIndex(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 8;
            var lsBookType = _context.TbBookTypes.AsNoTracking().OrderByDescending(x => x.TypeName);

            PagedList<TbBookType> models = new PagedList<TbBookType>(lsBookType, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        public IActionResult BookTypeCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookTypeCreate([Bind("TypeId,TypeName,Status,Sort,Thumb,ParentId")] TbBookType tbBookType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbBookType);
                await _context.SaveChangesAsync();
                _notyfService.Success("Đã thêm thể loại sách");
                return RedirectToAction(nameof(BookTypeIndex));
            }
            return View(tbBookType);
        }

        public async Task<IActionResult> BookTypeEdit(int? id)
        {
            if (id == null || _context.TbBookTypes == null)
            {
                return NotFound();
            }

            var tbBookType = await _context.TbBookTypes.FindAsync(id);
            if (tbBookType == null)
            {
                return NotFound();
            }
            return View(tbBookType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookTypeEdit(int id, [Bind("TypeId,TypeName,Status,Sort,Thumb,ParentId")] TbBookType tbBookType)
        {
            if (id != tbBookType.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbBookType);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Đã cập nhật thể loại sách");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBookTypeExists(tbBookType.TypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BookTypeIndex));
            }
            return View(tbBookType);
        }

        public async Task<IActionResult> BookTypeDelete(int? id)
        {
            if (id == null || _context.TbBookTypes == null)
            {
                return NotFound();
            }

            var tbBookType = await _context.TbBookTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (tbBookType == null)
            {
                return NotFound();
            }

            return View(tbBookType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookTypeDeleteConfirmed(int id)
        {
            if (_context.TbBookTypes == null)
            {
                return Problem("Entity set 'dbBookstoreContext.TbBookTypes'  is null.");
            }
            var tbBookType = await _context.TbBookTypes.FindAsync(id);
            if (tbBookType != null)
            {
                _context.TbBookTypes.Remove(tbBookType);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Error("Đã xóa thể loại sách");
            return RedirectToAction(nameof(BookTypeIndex));
        }

        private bool TbBookTypeExists(int id)
        {
          return _context.TbBookTypes.Any(e => e.TypeId == id);
        }
    }
}
