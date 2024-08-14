using BooksAPI.Data;
using BooksAPI.DTOs;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Controllers
{
    [Route("/api/reads")]
    public class ReadsController : Controller
    {
       private readonly AppDbContext _dbContext;

        public ReadsController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var reads = await _dbContext.Reads
            .Include(r => r.book)
            .ToListAsync();

            if (reads == null)
            {
                return NoContent();
            }

           var readDTO = new List<ReadDTO>();

            foreach (var read in reads)
            {
                readDTO.Add(new ReadDTO
                {
                    id = read.id,
                    current_page = read.current_page,
                    books = new BookDTO
                    {
                        id = read.book_id,
                        title = read.book.title,
                    }
                });
            }
            
            return Ok(readDTO);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] Read read)
        {

            var books = await _dbContext.Books.FindAsync(read.book_id);

            if (books == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            var users = await _dbContext.Users.FirstOrDefaultAsync(u => u.id == read.user_id);

            if (users == null)
            {
                return NotFound("O usuário não foi encontrado");
            }

           if (read.current_page < 0)
            {
                return BadRequest("A página atual não pode ser menor que 0");
            }

            var reads = await _dbContext.Reads.FirstOrDefaultAsync(r => r.book_id == read.book_id && r.user_id == read.user_id);

            if (reads != null)
            {
               _dbContext.Reads.Remove(reads);
            }

            _dbContext.Reads.Add(read);
            await _dbContext.SaveChangesAsync();

            return Created("Created", read);
        }
    }
}