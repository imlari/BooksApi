namespace BooksAPI.DTOs
{
    public class BookDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public ReadDTO? reads { get; set; }
    }
}