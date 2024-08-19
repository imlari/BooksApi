using BooksAPI.Data;
using BooksAPI.Models;
using BooksAPI.DTOs;
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
        public async Task<ActionResult> Create([FromBody] BookDTO book)
        {
            var newBook = new Book
            {
                title = book.title,
                author = book.author
            };

            _dbContext.Books.Add(newBook);
            await _dbContext.SaveChangesAsync();

            return Created("Created", newBook);
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
        public async Task<ActionResult> GetBookByTitle([FromBody] SearchByTitleDTO titleToSearch)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.title == titleToSearch.title);

            if (book == null)
            {
                return NotFound("O livro não foi encontrado");
            }

            return Ok(book);
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
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor([FromBody] SearchByAuthorDTO authorToSearch)
        {
            var books = await _dbContext.Books.Where(b => b.author == authorToSearch.author).ToListAsync();

            if (books.Count == 0)
            {
                return NotFound("Nenhum livro foi encontrado");
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