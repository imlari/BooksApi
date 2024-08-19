using BooksAPI.Data;
using BooksAPI.Models;
using BooksAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Controllers
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
            var users = await _dbContext.Users.OrderBy(u => u.id).ToListAsync();

            if (users.Count == 0)
            {
                return NoContent();
            }

            return Ok(users);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> Create([FromBody] CreateUserDTO user)
        {
            _dbContext.Users.Add(new User
            {
                name = user.name,
                email = user.email,
                password = user.password
            });
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
        public async Task<ActionResult> GetUserByEmail([FromBody] UserEmailDTO userEmail)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == userEmail.email);

            if (user == null)
            {
                return NotFound("O usuário não foi encontrado pelo email");
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(DeleteUserByIdDTO userToDelete)
        {
            var user = await _dbContext.Users.FindAsync(userToDelete.id);

            if (user == null)
            {
                return NotFound("O usuário não foi encontrado");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Usuário removido");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateUserDTO user)
        {
            var userToUpdate = await _dbContext.Users.FindAsync(id);

            if (userToUpdate == null)
            {
                return NotFound("O usuário não foi encontrado");
            }

            if (user.name == null && user.email == null && user.password == null)
            {
                return BadRequest("Nenhum dado foi informado para atualização");
            }

            if (user.name != null)
            {
                userToUpdate.name = user.name;
            }

            if (user.email != null)
            {
                userToUpdate.email = user.email;
            }

            if (user.password != null)
            {
                userToUpdate.password = user.password;
            }

            var userWithSameEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == user.email);

            if (userWithSameEmail != null)
            {
                return BadRequest("Já existe um usuário com esse email");
            }

            await _dbContext.SaveChangesAsync();

            return Ok("Usuário atualizado");
        }

        [HttpPost("login")]
        [Consumes("application/json")]
      
        public async Task<ActionResult> Login([FromBody] UserLoginDTO userLogin)
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