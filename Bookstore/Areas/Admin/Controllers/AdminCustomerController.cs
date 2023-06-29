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
    public class AdminCustomerController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminCustomerController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult CustomerIndex(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsCustomer = _context.TbCustomers.AsNoTracking().OrderByDescending(x => x.FullName);

            PagedList<TbCustomer> models = new PagedList<TbCustomer>(lsCustomer, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        public async Task<IActionResult> CustomerDetails(int? id)
        {
            if (id == null || _context.TbCustomers == null)
            {
                return NotFound();
            }

            var tbCustomer = await _context.TbCustomers
                .Include(t => t.Location)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (tbCustomer == null)
            {
                return NotFound();
            }

            return View(tbCustomer);
        }

        public async Task<IActionResult> CustomerEdit(int? id)
        {
            if (id == null || _context.TbCustomers == null)
            {
                return NotFound();
            }

            var tbCustomer = await _context.TbCustomers.FindAsync(id);
            if (tbCustomer == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.TbLocations, "LocationId", "LocationId", tbCustomer.LocationId);
            return View(tbCustomer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerEdit(int id, [Bind("CustomerId,FullName,BirthDate,Address,Email,Phone,LocationId,District,Ward,CreateDate,Password,Salt,Active")] TbCustomer tbCustomer)
        {
            if (id != tbCustomer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbCustomer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbCustomerExists(tbCustomer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.TbLocations, "LocationId", "LocationId", tbCustomer.LocationId);
            return View(tbCustomer);
        }


        private bool TbCustomerExists(int id)
        {
          return (_context.TbCustomers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
