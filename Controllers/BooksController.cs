using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Controllers
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

        [HttpPost("search-by-title")]
        [Consumes("application/json")]
        public async Task<ActionResult> GetBookByTitle([FromBody] string title)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.title == title);

            if (book == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            return Ok(book);
        }
        
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByFilter(Book book)
        {
            var books = await _dbContext.Books.Where(b => b.title == book.title || b.author == book.author).ToListAsync();

            if (books.Count == 0)
            {
                return NotFound($"Nenhum livro foi encontrado");
            }

            return Ok(books);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> PutBook(int id, Book book)
        {
            var bookToUpdate = await _dbContext.Books.Where(b => b.id == id).FirstOrDefaultAsync();

            if (bookToUpdate == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            if (book.title != null)
            {
                bookToUpdate.title = book.title;
            }

            if (book.author != null)
            {
                bookToUpdate.author = book.author;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(bookToUpdate);
        }

        [HttpPost("search-by-author")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor([FromBody] string author)
        {
            var books = await _dbContext.Books.Where(b => b.author == author).ToListAsync();

            if (books.Count == 0)
            {
                return NotFound($"Nenhum livro foi encontrado");
            }

            return Ok(books);
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