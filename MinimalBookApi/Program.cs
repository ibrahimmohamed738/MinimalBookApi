var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var books = new List<Book>
{
    new() { Id=1, Title="The Hitchhiker's Guide to the Galaxy", Auther="Douglas Adams" },
    new() { Id=2, Title="1984", Auther="George Orwell" },
    new() { Id=3, Title="Ready Player One", Auther="Ernest Cline" },
    new() { Id=4, Title="The Martian", Auther="Andy Weir" },
};

app.MapGet("/book", () =>
{
    return books;
});

app.MapGet("/book/{id}", (int id) =>
{
    var book = books.Find(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Sorry, Book not found.");
    return Results.Ok(book);
});

app.MapPost("/book", (Book book) =>
{
    books.Add(book);
    return Results.Ok(books);
});

app.MapPut("/book/{id}", (Book updatedBook, int id) =>
{
    var book = books.Find(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Sorry, Book not found.");

    book.Title = updatedBook.Title;
    book.Auther = updatedBook.Auther;

    return Results.Ok(books);
});


app.MapDelete("/book/{id}", (int id) =>
{
    var book = books.Find(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Sorry, Book not found.");

    books.Remove(book);

    return Results.Ok(books);
});

app.Run();

class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Auther { get; set; }
}

