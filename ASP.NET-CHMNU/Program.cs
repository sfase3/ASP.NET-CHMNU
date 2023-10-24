var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var IvanCompany = new Company
{
Name = "IvKov DEV",
CompanyType = "MMM Pyramide"
};

// endpoint 1
app.MapGet("/", () =>
{
var companyData = "Company name: " + IvanCompany.Name + ", type: " + IvanCompany.CompanyType;
return companyData;
});

// endpoint 2
app.MapGet("/random-num", () =>
{
var randomNumber = new Random().Next(0, 101);
return "Random number: " + randomNumber;
});


app.Run();

public class Company
{
    public string? Name { get; set; }
    public string? CompanyType { get; set; }
}

