using BlazorHtmxWeb.Features.Contacts;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddCookie();
builder.Services.AddAuthorization();

builder.Services.AddRazorComponents();
builder.Services.AddCarter();

builder.Services.AddSingleton<ContactsRepository>();

var app = builder.Build();

SeedData();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapGet("/", () => Results.Redirect("/contacts"));

app.MapCarter();
app.Run();


void SeedData()
{
    var contacts = new List<Contact>
    {
        new Contact
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@doe.com",
            City = "New York",
            Phone = "555-555-5555"
        },
        new Contact
        {
            Id = 2,
            Name = "Jane Doe",
            Email = "jane@doe.com",
            City = "New York",
            Phone = "555-555-5555"
        },
    };

    using var scope = app.Services.CreateScope();
    var repository = scope.ServiceProvider.GetRequiredService<ContactsRepository>();
    repository.Contacts.AddRange(contacts);    
}