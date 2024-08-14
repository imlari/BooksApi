namespace UsersAPI.Models
{
    public class User
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    
    public class UserLogin
        {
            public string email { get; set; }
            public string password { get; set; }
        }
}