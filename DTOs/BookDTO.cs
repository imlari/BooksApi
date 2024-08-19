namespace BooksAPI.DTOs
{
    public class BookDTO
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
    }

    public class SearchByTitleDTO
    {
        public string title { get; set; }
    }

    public class SearchByAuthorDTO
    {
        public string author { get; set; }
    }
}