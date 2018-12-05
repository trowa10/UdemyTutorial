using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Controllers
{
    public class BooksController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Books.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Add(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);
            if (book is null)
                return NotFound();

            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);
            if (book is null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);
            if (book is null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")] // Delete but you othert than delete you are updateing something
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveBook(int id)
        {
            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
        }
    }
}