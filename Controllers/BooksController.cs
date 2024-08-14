using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Controllers
{
    [ApiController]
    [Route("/api/books")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public BooksController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var books = await _dbContext.Books.ToListAsync();

            if (books.Count == 0)
            {
                return NoContent();
            }

            return Ok(books);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> Create([FromBody] Book book)
        {
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return Created("Created", book);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            return Ok(book);
        }

        [HttpPost("title")]
        public async Task<ActionResult<Book>> GetBookByTitle(string title)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == title);

            if (book == null)
            {
                return NotFound($"O livro '{title}' não foi encontrado");
            }

            return Ok(book);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByFilter(Book book)
        {
            var books = await _dbContext.Books.Where(b => b.Title == book.Title || b.Author == book.Author).ToListAsync();

            if (books.Count == 0)
            {
                return NotFound($"Nenhum livro foi encontrado");
            }

            return Ok(books);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> PutBook(int id, Book book)
        {
            var bookToUpdate = await _dbContext.Books.FindAsync(id);

            if (bookToUpdate == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            bookToUpdate.Title = book.Title;
            bookToUpdate.Author = book.Author;

            await _dbContext.SaveChangesAsync();

            return Ok(bookToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return Ok(book);
        }
    }
}