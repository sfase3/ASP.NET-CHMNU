using Microsoft.AspNetCore.Http;
using MyApp.schemas;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
var app = builder.Build();
builder.Configuration.AddJsonFile("config/data.json");

app.Map("/users/{id:int?}", async (int? id, HttpContext context, IConfiguration appConfig) =>
{
    Member[] members = appConfig.GetSection("libraryInfo:members").Get<Member[]>();
    StringBuilder sb = new StringBuilder();
    sb.Append("<html><head><style>");
    sb.Append("body { font-family: Arial, sans-serif; background-color: #f0f0f0; margin: 20px; }");
    sb.Append(".container { max-width: 800px; margin: 0 auto; padding: 20px; background-color: #fff; box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); }");
    sb.Append("h3 { color: #333; }");
    sb.Append("p { margin: 10px 0; }");
    sb.Append("</style></head><body>");

    if (id.HasValue && members != null && id.Value >= 0 && id.Value < members.Length)
    {
        sb.Append("<div class='container'><h3>User's Profile</h3>");
        sb.Append($"<p>Name: {members[id.Value].FullName}</p>");
        sb.Append($"<p>Membership level: {members[id.Value].MembershipLevel}</p>");
        sb.Append($"<p>Library Card Number: {members[id.Value].LibraryCardNumber}</p>");
        sb.Append("</div>");
    }

    sb.Append("</body></html>");
    await context.Response.WriteAsync(sb.ToString());
});

app.Map("/library-main", async (HttpContext context, IConfiguration appConfig) =>
{
    StringBuilder sb = new StringBuilder();
    sb.Append("<html><head><style>");
    sb.Append("body { font-family: Arial, sans-serif; background-color: #f0f0f0; margin: 20px; }");
    sb.Append(".container { max-width: 800px; margin: 0 auto; padding: 20px; background-color: #fff; box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); }");
    sb.Append("h3 { color: #333; }");
    sb.Append("p { margin: 10px 0; }");
    sb.Append("</style></head><body>");

    sb.Append("<div class='container'><h3>Welcome to the Library</h3>");
    sb.Append("</div>");
    sb.Append("</body></html>");
    await context.Response.WriteAsync(sb.ToString());
});

app.Map("/library/books", async (HttpContext context, IConfiguration appConfig) =>
{
    Book[] books = appConfig.GetSection("libraryInfo:bookCollection").Get<Book[]>();
    StringBuilder sb = new StringBuilder();
    sb.Append("<html><head><style>");
    sb.Append("body { font-family: Arial, sans-serif; background-color: #f0f0f0; margin: 20px; }");
    sb.Append(".container { max-width: 800px; margin: 0 auto; padding: 20px; background-color: #fff; box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); }");
    sb.Append("h3 { color: #333; }");
    sb.Append("p { margin: 10px 0; }");
    sb.Append("</style></head><body>");

    sb.Append("<div class='container'><h3>Library Books</h3>");
    foreach (var book in books)
    {
        sb.Append($"<p>Book Title: {book.Title}</p>");
        sb.Append($"<p>Publishing year: {book.PublishedYear}</p>");
        sb.Append($"<p>Author: {book.Author}</p>");
    }
    sb.Append("</div>");
    sb.Append("</body></html>");
    await context.Response.WriteAsync(sb.ToString());
});

app.Map("/", async (HttpContext context, IConfiguration appConfig) =>
{
    Member[] members = appConfig.GetSection("libraryInfo:members").Get<Member[]>();
    StringBuilder sb = new StringBuilder();
    sb.Append("<html><head><style>");
    sb.Append("body { font-family: Arial, sans-serif; background-color: #f0f0f0; margin: 20px; }");
    sb.Append(".container { max-width: 800px; margin: 0 auto; padding: 20px; background-color: #fff; box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); }");
    sb.Append("h3 { color: #333; }");
    sb.Append("p { margin: 10px 0; }");
    sb.Append("</style></head><body>");

    sb.Append("<div class='container'><h3>Welcome to the Library</h3>");
    sb.Append("<ul>");
    sb.Append("<li><a href='/library-main'>Visit the Library</a></li>");
    sb.Append("<li><a href='/library/books'>Explore Books</a></li>");
    for (int i = 0; i < members.Length; i++)
    {
        sb.Append($"<li><a href='/users/{i}'>{members[i].FullName}'s Page</a></li>");
    }
    sb.Append("</ul>");
    sb.Append("</div>");
    sb.Append("</body></html>");
    await context.Response.WriteAsync(sb.ToString());
});

app.Use(async (context, next) =>
{
    await next.Invoke();
    if (context.Response.StatusCode == 404)
        await context.Response.WriteAsync("Resource Not Found");
});

app.Run();

