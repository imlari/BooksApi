using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Criar rota para listagem de livros
app.MapGet("/api/books", async (AppDbContext db) => {
    var books = await db.Books.ToListAsync();
    return Results.Ok(books);
});

// Criar rota para realizar um envio de novos livros
app.MapPost("/api/books", async (AppDbContext db, Book book) => {
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created("Created", book);
});

//Criar rota para retornar um livro específico
app.MapGet("/api/books/{id}", (int id) => {
    return Results.Ok($"Você está na rota de retorno do livro de id {id}");

});

//Criar rota para atualizar um livro específico
app.MapPut("/api/books/{id}", (int id) => {
    return Results.Ok($"Você está na rota de atualização do livro de id {id}");
});

//Criar rota para deletar um livro específico
app.MapDelete("/api/books/{id}", (int id) => {
    return Results.Ok($"Você está na rota de deleção do livro de id {id}");
});

app.Run();
