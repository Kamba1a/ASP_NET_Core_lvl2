
namespace WebStore.Domain.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PagesNumber { get; set; }
        public int Year { get; set; }
    }
}