using AngularGridPagination.Data;
using AngularGridPagination.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(nameof(AppDbContext)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapGet("api/customers", async (int pageSize, int skip, AppDbContext context) =>
    {
        var totalCustomers = await context.Customers.CountAsync();
        var customers = await context.Customers
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();


        return new PagedResult<Customer>
        {
            Results = customers,
            TotalItems = totalCustomers
        };
    });

app.MapFallbackToFile("index.html");

await SeedData();

app.Run();




async Task SeedData()
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


    for (int i = 0; i < 100; i++)
    {
        var newCustomer = new Customer
        {
            FullName = $"Customer #{i}",
            Email = $"Email #{i}"
        };

        context.Add(newCustomer);
    }

    await context.SaveChangesAsync();
}