namespace BooksAPI.Models
{
    public class Book
    {
        public int id { get; set; }
        public string title { get; set; }
        public string? author { get; set; }
        public ICollection<Read>? Reads { get; set; }
    }
}