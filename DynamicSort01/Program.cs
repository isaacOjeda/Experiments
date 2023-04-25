using Bogus;
using DynamicSort01;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseInMemoryDatabase(nameof(MyDbContext)));


var app = builder.Build();

await SeedData();


app.MapGet("/api/customers", (string sort_dir, string sort_field, MyDbContext context) =>
{
    return context.Customers
        .AsQueryable()
        .OrderBy($"{sort_field} {sort_dir}")
        .ToListAsync();
});

app.Run();


async Task SeedData()
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<MyDbContext>();

    foreach (var item in Enumerable.Range(1, 100))
    {
        var testUser = new Faker<Customer>()
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Age, f => f.Hacker.Random.Int(min: 18, max: 80));

        var user = testUser.Generate();
        context.Customers.Add(user);
    }

    await context.SaveChangesAsync();
}

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {

    }

    public DbSet<Customer> Customers => Set<Customer>();
}

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = default!;
    public int Age { get; set; }
}