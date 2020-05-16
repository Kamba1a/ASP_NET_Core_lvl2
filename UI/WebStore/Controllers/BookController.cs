using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.Controllers
{
    public class BookController : Controller
    {
        IObjectService<BookViewModel> _books;

        public BookController(IObjectService<BookViewModel> books)
        {
            _books = books;
        }

        // GET: /<controller>/Books
        public IActionResult Books()
        {
            return View(_books.GetAll());
        }

        // GET: /<controller>/BookDetails
        public IActionResult BookDetails(int id)
        {
            return View(_books.GetById(id));
        }

        // GET: /<controller>/BookDetails/{id?}
        public IActionResult BookEdit(int? id)
        {
            if (!id.HasValue) return View(new BookViewModel());
            BookViewModel book = _books.GetById(id.Value);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public IActionResult BookEdit(BookViewModel _book)
        {
            BookViewModel book = _books.GetById(_book.Id);
            if (book == null) _books.AddNew(_book);
            else
            {
                book.Title = _book.Title;
                book.Author = _book.Author;
                book.PagesNumber = _book.PagesNumber;
                book.Year = _book.Year;
            }
            _books.Commit();
            return RedirectToAction("Books");
        }

        public IActionResult BookDelete(int id)
        {
            _books.Delete(id);
            return RedirectToAction("Books");
        }
    }
}
