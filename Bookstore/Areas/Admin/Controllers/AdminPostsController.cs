using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Entity;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;
using Bookstore.Helpper;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPostsController : Controller
    {
		private readonly dbBookstoreContext _context;
		public INotyfService _notyfService { get; }

		public AdminPostsController(dbBookstoreContext context, INotyfService notyfService)
		{
			_context = context;
			_notyfService = notyfService;
        }
        public IActionResult PostIndex(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 8;
			var lsPost = _context.TbPosts.AsNoTracking().OrderByDescending(x => x.PostId);

			PagedList<TbPost> models = new PagedList<TbPost>(lsPost, pageNumber, pageSize);

			ViewBag.CurrentPage = pageNumber;
			return View(models);
		}

        public async Task<IActionResult> PostDetails(int? id)
        {
            if (id == null || _context.TbPosts == null)
            {
                return NotFound();
            }

            var tbPost = await _context.TbPosts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tbPost == null)
            {
                return NotFound();
            }

            return View(tbPost);
        }

        public IActionResult PostCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCreate([Bind("PostId,Title,Scontents,Contents,Thumb,Status,CreatedDate,Tags,IsHot,IsNewfeed,Views,Active")] TbPost tbPost, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                tbPost.Title = Utilities.ToTitleCase(tbPost.Title);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(tbPost.Title) + extension;
                    tbPost.Thumb = await Utilities.UploadFile(fThumb, @"post", image.ToLower());
                }
                if (string.IsNullOrEmpty(tbPost.Thumb)) tbPost.Thumb = "default-book.jpg";

                tbPost.CreatedDate = DateTime.Now;
                _context.Add(tbPost);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm tin tức thành công");
                return RedirectToAction(nameof(PostIndex));
            }
            return View(tbPost);
        }

        public async Task<IActionResult> PostEdit(int? id)
        {
            if (id == null || _context.TbPosts == null)
            {
                return NotFound();
            }

            var tbPost = await _context.TbPosts.FindAsync(id);
            if (tbPost == null)
            {
                return NotFound();
            }
            return View(tbPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEdit(int id, [Bind("PostId,Title,Scontents,Contents,Thumb,Status,CreatedDate,Tags,IsHot,IsNewfeed,Views,Active")] TbPost tbPost, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != tbPost.PostId)
            {
                return NotFound();
            }

            try
            {
                tbPost.Title = Utilities.ToTitleCase(tbPost.Title);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(tbPost.Title) + extension;
                    tbPost.Thumb = await Utilities.UploadFile(fThumb, @"post", image.ToLower());
                }
                if (string.IsNullOrEmpty(tbPost.Thumb)) tbPost.Thumb = "default-book.jpg";

                tbPost.CreatedDate = DateTime.Now;
                _context.Update(tbPost);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbPostExists(tbPost.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(PostIndex));
        }

        public async Task<IActionResult> PostDelete(int? id)
        {
            if (id == null || _context.TbPosts == null)
            {
                return NotFound();
            }

            var tbPost = await _context.TbPosts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tbPost == null)
            {
                return NotFound();
            }

            return View(tbPost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbPosts == null)
            {
                return Problem("Entity set 'dbBookstoreContext.TbPosts'  is null.");
            }
            var tbPost = await _context.TbPosts.FindAsync(id);
            if (tbPost != null)
            {
                _context.TbPosts.Remove(tbPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbPostExists(int id)
        {
          return (_context.TbPosts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
