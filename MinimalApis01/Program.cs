using MinimalApis01.Features.Products;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGroup("api/products")
    .MapProductsApi();


app.Run();
