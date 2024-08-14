namespace BooksAPI.Models
{
    public class Read
    {
        public int id { get; set; }
        public int current_page { get; set; }
        public int book_id { get; set; }
        public int? user_id { get; set; }
        public Book book { get; set; }
        public User? user { get; set; }
    }
}