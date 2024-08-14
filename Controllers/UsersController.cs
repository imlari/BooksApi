using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UsersAPI.Controllers
{
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public UsersController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var users = await _dbContext.Users.ToListAsync();

            if (users.Count == 0)
            {
                return NoContent();
            }

            return Ok(users);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Created("Created", user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("O usuário não foi encontrado");
            }

            return Ok(user);
        }

        [HttpPost("email")]
        [Consumes("application/json")]
        public async Task<ActionResult> GetUserByEmail([FromBody] string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
            {
                return NotFound("O usuário não foi encontrado pelo email");
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("O usuário não foi encontrado");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Usuário removido");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] User user)
        {
            var userToUpdate = await _dbContext.Users.FindAsync(id);

            if (userToUpdate == null)
            {
                return NotFound("O usuário não foi encontrado");
            }

            userToUpdate.name = user.name;
            userToUpdate.email = user.email;
            userToUpdate.password = user.password;

            await _dbContext.SaveChangesAsync();

            return Ok("Usuário atualizado");
        }

        [HttpPost("login")]
        [Consumes("application/json")]
      
        public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
        {
            var userToLogin = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == userLogin.email && u.password == userLogin.password);

            if (userToLogin == null)
            {
                return NotFound("Usuário não encontrado");
            }

            return Ok("Usuário logado");
        }
    }
}