using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDwithIdentity.Data;
using CRUDwithIdentity.Models;
using CRUDwithIdentity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Text.Encodings.Web;

namespace CRUDwithIdentity.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return HtmlEncoder.Default.Encode(input);
        }

        // GET: Book/AddOrEdit/
        public async Task<IActionResult> AddOrEdit(int? id)
        {
            BookViewModel bookViewModel;
            try
            {
                if (id == null || id == 0)
                {
                    bookViewModel = new BookViewModel();
                }
                else
                {
                    var bookEntity = await _context.Books.FindAsync(id);
                    if (bookEntity != null)
                    {
                        bookViewModel = new BookViewModel
                        {
                            BookID = bookEntity.BookID,
                            Title = bookEntity.Title,
                            Author = bookEntity.Author,
                            Price = bookEntity.Price,
                        };
                    }
                    else
                    {
                        bookViewModel = new BookViewModel();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                bookViewModel = new BookViewModel();
                ModelState.AddModelError("", "An error occurred while loading the book data.");
            }
            return View(bookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEdit([Bind("BookID,Title,Author,Price")] BookViewModel bookViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bookViewModel.Title = SanitizeInput(bookViewModel.Title);
                    bookViewModel.Author = SanitizeInput(bookViewModel.Author);
          

                    if (bookViewModel.BookID > 0)
                    {
                        var existingBook = await _context.Books.FindAsync(bookViewModel.BookID);
                        if (existingBook == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            existingBook.Title = bookViewModel.Title;
                            existingBook.Author = bookViewModel.Author;
                            existingBook.Price = bookViewModel.Price;
                        }
                    }
                    else
                    {
                        BookEntity newBook = new BookEntity
                        {
                            Title = bookViewModel.Title,
                            Author = bookViewModel.Author,
                            Price = bookViewModel.Price
                        };

                        await _context.Books.AddAsync(newBook);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                ModelState.AddModelError("", "An error occurred while saving the book data.");
            }

            return View(bookViewModel);
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                return View(books);
            }
            catch (Exception ex)
            {
            
                Console.WriteLine($"An error occurred: {ex.Message}");

          
                ModelState.AddModelError("", "An error occurred while loading the list of books.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BookViewModel bookViewModel)
        {
            try
            {
                var book = await _context.Books.FindAsync(bookViewModel.BookID);
                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex.Message}");

        
                ModelState.AddModelError("", "An error occurred while deleting the book.");
                return RedirectToAction("List");
            }
        }
    }
}
