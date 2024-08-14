namespace BooksAPI.DTOs
{
    public class ReadDTO
    {
        public int id { get; set; }
        public int current_page { get; set; }
        public BookDTO? books { get; set; }
    }
}