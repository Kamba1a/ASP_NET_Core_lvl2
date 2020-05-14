using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class InMemoryBooksData: IitemData<BookViewModel>
    {
        List<BookViewModel> _books;

        public InMemoryBooksData()
        {
            _books = new List<BookViewModel>{
                new BookViewModel{Id=1, Title="Все о нашей вселенной", Author="Иванов", PagesNumber=300, Year=2017},
                new BookViewModel{Id=2, Title="Жизнь на планете Земля", Author="Петров", PagesNumber=150, Year=2015}
            };
        }

        public IEnumerable<BookViewModel> GetAll()
        {
            return _books;
        }

        public BookViewModel GetById(int id)
        {
            return _books.FirstOrDefault(book => book.Id == id);
        }

        public void AddNew(BookViewModel book)
        {
            if (_books.Count > 0) book.Id = _books.Last().Id + 1;
            else book.Id = 1;
            _books.Add(book);
        }

        public void Delete(int id)
        {
            BookViewModel book = GetById(id);
            if (book != null) _books.Remove(GetById(id));
        }

        public void Commit()
        {

        }
    }
}
