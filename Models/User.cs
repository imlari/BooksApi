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

    
    public class UserLoginDTO
        {
            public string email { get; set; }
            public string password { get; set; }
        }

    public class CreateUserDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class UserEmailDTO
    {
        public string email { get; set; }
    }

    public class DeleteUserByIdDTO
    {
        public string id { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}