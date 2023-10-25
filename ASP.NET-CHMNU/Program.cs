using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging;
using MyApp.loggers;

var builder = WebApplication.CreateBuilder();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logs.txt"));

var app = builder.Build();
var logger = app.Logger;

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    if (path == "/api/cookie" && request.Method == "POST")
    {
        try
        {
            var cookieRecord = await request.ReadFromJsonAsync<CookieRecord>();
            if (cookieRecord != null)
            {
                var expiresAt = DateTime.Parse(cookieRecord.ExpiresAt);
                var cookieOptions = new CookieOptions
                {
                    Expires = expiresAt
                };
                context.Response.Cookies.Append(cookieRecord.Value, cookieRecord.Value, cookieOptions);
                await response.WriteAsJsonAsync(new { message = "Cookie was added" });
            }
            else
            {
                throw new Exception("Invalid data");
            }
        }
        catch (Exception)
        {
            response.StatusCode = 400;
            app.Logger.LogError($"LogError {path} - Invalid data");
            await response.WriteAsJsonAsync(new { message = "Invalid data" });
        }
    }


    else if (path == "/api/check-cookie" && request.Method == "POST")
    {
        try
        {
            var cookieRecordName = await request.ReadFromJsonAsync<CookieName>();
            if (cookieRecordName.Value != null)
            {
                if (context.Request.Cookies.ContainsKey(cookieRecordName.Value))
                {
                    await response.WriteAsJsonAsync(new { message = "Cookie exists" });
                }
                else
                {
                    throw new Exception("Cookie doesn't exist " + cookieRecordName.Value);
                }
            }
            else
            {
                throw new Exception("Invalid data");
            }
        }
        catch (Exception err)
        {
            response.StatusCode = 404;
            app.Logger.LogError(err.Message);
        }
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("index.html");
    }
});

app.Run();

public class CookieRecord
{
    public string Value { get; set; } = "";
    public string ExpiresAt { get; set; }
}

public class CookieName
{
    public string Value { get; set; } = "";
}