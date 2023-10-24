var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.
    Configuration.
    AddJsonFile("config/google.json").
    AddXmlFile("config/apple.xml").
    AddIniFile("config/microsoft.ini");

app.MapGet("/", () => "Hello, World!");

//Завдання 1 - сервіс, який аналізує кількість співробітників
app.Map("/company", (IConfiguration appConfig) =>
{
    var name = "";
    var employees = 0;
    IConfigurationSection copmanyConfig = appConfig.GetSection("Company");
    foreach (var item in copmanyConfig.GetChildren())
    {
        var currentName = item.Key;
        var amount = int.Parse(item.GetSection("EmployeesAmount").Value);
        if (amount > employees)
        {
            name = currentName;
            employees = amount;
        }
    }
    return $"{name}: {employees}";

});

//Завдання 2 - підключення json-файл конфігурації в якому в кількох полях записано деякі дані про Вас
builder.Configuration.AddJsonFile("config/me.json");

app.Map("/me", (IConfiguration appConfig) => $" Ім'я: {appConfig["name"]}\n " +
        $"Прізвище: {appConfig["surname"]}\n " +
        $"Вік: {appConfig["age"]}");

app.Run();
