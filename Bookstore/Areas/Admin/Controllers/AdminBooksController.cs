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
using Bookstore.Helpper;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBooksController : Controller
    {
        private readonly dbBookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminBooksController(dbBookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult BookIndex(int page = 1, int TypeID = 0)
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
            ViewData["NXB"] = new SelectList(_context.TbPublishers, "PublisherId", "PublisherName");
            ViewData["TheLoai"] = new SelectList(_context.TbBookTypes, "TypeId", "TypeName", TypeID);
            return View(models);
        }

        public IActionResult Filtter(int TypeID = 0)
        {
            var url = $"/Admin/AdminBooks/BookIndex?TypeID={TypeID}";
            if (TypeID == 0)
            {
                url = $"/Admin/AdminBooks/BookIndex";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        public async Task<IActionResult> BookDetails(int? id)
        {
            if (id == null || _context.TbBooks == null)
            {
                return NotFound();
            }

            var tbBook = await _context.TbBooks
                .Include(t => t.Publisher)
                .Include(t => t.Type)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (tbBook == null)
            {
                return NotFound();
            }

            return View(tbBook);
        }

        public IActionResult BookCreate()
        {
            ViewData["NXB"] = new SelectList(_context.TbPublishers, "PublisherId", "PublisherName");
            ViewData["TheLoai"] = new SelectList(_context.TbBookTypes, "TypeId", "TypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookCreate([Bind("BookId,BookName,Author,Desription,Status,IsHot,Image,ListImage,Quantity,Price,PromotionPrice,Language,Translator,Page,PublishYear,TypeId,PublisherId")] TbBook Book, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                Book.BookName = Utilities.ToTitleCase(Book.BookName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(Book.BookName) + extension;
                    Book.Image = await Utilities.UploadFile(fThumb, @"books", image.ToLower());
                }
                if (string.IsNullOrEmpty(Book.Image)) Book.Image = "default-book.jpg";

                _context.Add(Book);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm sách thành công");
                return RedirectToAction(nameof(BookIndex));
            }
            ViewData["NXB"] = new SelectList(_context.TbPublishers, "PublisherId", "PublisherName", Book.PublisherId);
            ViewData["TheLoai"] = new SelectList(_context.TbBookTypes, "TypeId", "TypeName", Book.TypeId);
            return View(Book);
        }

        public async Task<IActionResult> BookEdit(int? id)
        {
            if (id == null || _context.TbBooks == null)
            {
                return NotFound();
            }

            var Book = await _context.TbBooks.FindAsync(id);
            if (Book == null)
            {
                return NotFound();
            }
            ViewData["NXB"] = new SelectList(_context.TbPublishers, "PublisherId", "PublisherName");
            ViewData["TheLoai"] = new SelectList(_context.TbBookTypes, "TypeId", "TypeName");
            return View(Book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookEdit(int id, [Bind("BookId,BookName,Author,Desription,Status,IsHot,Image,ListImage,Quantity,Price,PromotionPrice,Language,Translator,Page,PublishYear,TypeId,PublisherId")] TbBook Book, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != Book.BookId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    Book.BookName = Utilities.ToTitleCase(Book.BookName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(Book.BookName) + extension;
                        Book.Image = await Utilities.UploadFile(fThumb, @"books", image.ToLower());
                    }
                    //if (string.IsNullOrEmpty(Book.Image)) Book.Image = Book.Image;
                    _context.Update(Book);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật sách thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBookExists(Book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BookIndex));
            //}
            //ViewData["NXB"] = new SelectList(_context.TbPublishers, "PublisherId", "PublisherName", Book.PublisherId);
            //ViewData["TheLoai"] = new SelectList(_context.TbBookTypes, "TypeId", "TypeName", Book.TypeId);
            //return View(Book);
        }

        private bool TbBookExists(int id)
        {
          return (_context.TbBooks?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
