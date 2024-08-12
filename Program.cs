using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Criar rota para listagem de livros
app.MapGet("/api/books", async (AppDbContext db) => {
    var books = await db.Books.ToListAsync();
    
    if (books.Count == 0)
    {
        return Results.NoContent();
    }
    return Results.Ok(books);
});

// Criar rota para realizar um envio de novos livros
app.MapPost("/api/books", async (AppDbContext db, Book book) => {
    db.Books.Add(book);
    await db.SaveChangesAsync();
    // Created = 201
    return Results.Created("Created", book);
});

//Criar rota para retornar um livro específico
app.MapGet("api/books/{id}", async (AppDbContext db, int id) => {
    var book = await db.Books.FirstOrDefaultAsync(b => b.id == id);

    if (book == null)
    {
        return Results.NotFound("O livro não foi encontrado");
    }
    // Ok = 200
    return Results.Ok(book);
});

app.MapGet("api/books/title/{title}", async (AppDbContext db, string Title) => {
    var book = await db.Books.FirstOrDefaultAsync(b => b.Title == Title);

    if (book == null)
    {
        return Results.NotFound($"O livro '{Title}' não foi encontrado");
    }

    return Results.Ok(book);
});

app.MapGet("api/books/author/{author}", async (AppDbContext db, string Author) => {
    var booksByAuthor = await db.Books.Where(b => b.Author == Author).ToListAsync();

    if (booksByAuthor.Count == 0)
    {
        return Results.NotFound($"Nenhum livro do autor '{Author}' foi encontrado");
    }

    return Results.Ok(booksByAuthor);
});

//Criar rota para atualizar um livro específico
app.MapPut("/api/books/{id}", async (AppDbContext db, int id, Book book) => {
    var bookToUpdate = await db.Books.FindAsync(id);
    if (bookToUpdate == null)
    {
        return Results.NotFound("O livro não foi encontrado");
    }
    bookToUpdate.Title = book.Title;
    bookToUpdate.Author = book.Author;
    await db.SaveChangesAsync();
    return Results.Ok(bookToUpdate);
});

//Criar rota para deletar um livro específico
app.MapDelete("/api/books/{id}", async (AppDbContext db, int id) => {
   var bookToDelete = await db.Books.FindAsync(id);

   if (bookToDelete == null)
   {
       return Results.NotFound("O livro não foi encontrado");
   }

   db.Books.Remove(bookToDelete);
   await db.SaveChangesAsync();
   return Results.Ok($"O livro '{bookToDelete.Title}' foi deletado");
});

app.Run();
