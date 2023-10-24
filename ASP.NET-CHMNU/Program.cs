using MyApp.services.CalcService;
using MyApp.services.TimeService;
using System.Text;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddTransient<ICalcService, CalcService>()
    .AddTransient<ITimeService, TimeService>();

var app = builder.Build();

app.MapPost("/calc", async context =>
{
    ICalcService? calcService = context.RequestServices.GetService<ICalcService>();
    var form = await context.Request.ReadFormAsync();
    var num1 = int.Parse(form["num1"]);
    var num2 = int.Parse(form["num2"]);
    var operation = form["operation"];
    var port = context.Request.Host.Port;
    float result = 0;

    switch (operation)
    {
        case "+":
        default:
            result = calcService.Sum(num1, num2);
            break;
        case "-":
            result = calcService.Subtract(num1, num2);
            break;
        case "*":
            result = calcService.Multiply(num1, num2);
            break;
        case "/":
            result = calcService.Divide(num1, num2);
            break;
    }

    context.Response.ContentType = "text/html;charset=utf-8";
    var responseHtml =
    $"<h2>Answer: {result}</h2>";
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync(responseHtml);
});

app.MapGet("/", async context =>
{
    var sb = new StringBuilder();

    sb.Append(
        "<div style=\"text-align: center;\">" +
            "<h1>Simple Calculator</h1>" +
            "<form method=\"post\" action=\"/calc\">" +
                "<div style=\"margin: 10px;\">" +
                    "<input type=\"number\" name=\"num1\" placeholder=\"Enter a number\" required>" +
                "</div>" +
                "<div style=\"margin: 10px;\">" +
                    "<select name=\"operation\" required>" +
                        "<option value=\"+\">Addition (+)</option>" +
                        "<option value=\"-\">Subtraction (-)</option>" +
                        "<option value=\"*\">Multiplication (*)</option>" +
                        "<option value=\"/\">Division (/)</option>" +
                    "</select>" +
                "</div>" +
                "<div style=\"margin: 10px;\">" +
                    "<input type=\"number\" name=\"num2\" placeholder=\"Enter another number\" required>" +
                "</div>" +
                "<button type=\"submit\">" +
                    "Calculate" +
                "</button>" +
            "</form>" +
        "</div>"
        );

    context.Response.ContentType = "text/html;charset=utf-8";

    await context.Response.WriteAsync(sb.ToString());
});


app.MapGet("/gettime", async context =>
{
    ITimeService? timeService = context.RequestServices.GetService<ITimeService>();
    string datePhrase = timeService.GetDatePhrase();

    var pageContent = new StringBuilder();
    pageContent.Append(
        "<html>" +
        "<head>" +
        "<title>Current Time</title>" +
        "</head>" +
        "<body style='font-family: Arial, sans-serif; text-align: center;'>" +
        $"<h1>Hello</h1>" +
        $"<p>{datePhrase}</p>" +
        "</body>" +
        "</html>");

    context.Response.ContentType = "text/html;charset=utf-8";
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync(pageContent.ToString());
});


app.Run();
