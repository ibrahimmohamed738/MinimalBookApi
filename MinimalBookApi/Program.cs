using Microsoft.EntityFrameworkCore;
using MinimalBookApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookDb"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var books = new List<Book>
//{
//    new() { Id=1, Title="The Hitchhiker's Guide to the Galaxy", Auther="Douglas Adams" },
//    new() { Id=2, Title="1984", Auther="George Orwell" },
//    new() { Id=3, Title="Ready Player One", Auther="Ernest Cline" },
//    new() { Id=4, Title="The Martian", Auther="Andy Weir" },
//};

app.MapGet("/book", async (DataContext context) =>
{
    return await context.Books.ToListAsync();
});

app.MapGet("/book/{id}", async (DataContext context, int id) =>
  await context.Books.FindAsync(id) is Book book ?
  Results.Ok(book)
  : Results.NotFound("Sorry book not found."));


app.MapPost("/book", async (DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

app.MapPut("/book/{id}", async (DataContext context, Book updatedBook, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, Book not found.");

    book.Title = updatedBook.Title;
    book.Auther = updatedBook.Auther;

    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});


app.MapDelete("/book/{id}", async (DataContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, Book not found.");

    context.Books.Remove(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

app.Run();

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Auther { get; set; }
}

