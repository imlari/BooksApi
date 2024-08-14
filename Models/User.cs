namespace BooksAPI.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public ICollection<Read>? Reads { get; set; }
    }

    
    public class UserLogin
        {
            public string email { get; set; }
            public string password { get; set; }
        }
}