




using Syncfusion.EJ2.DocumentEditor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


app.MapPost("upload", (IFormFile file, ILogger<Program> log) =>
{
    log.LogInformation("Uploading file");
    log.LogInformation($"File name: {file.FileName}");
    log.LogInformation($"File length: {file.Length}");
});

app.MapPost("SystemClipboard ", (Clipboard clipboard) =>
{
    // syncfusion word processor paste with format handler


});

app.MapGet("demo", () =>
{
    using var fileSream = File.Open("46.docx", FileMode.Open);
    
    var document = WordDocument.Load(fileSream, FormatType.Docx);

    string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);

    document.Dispose();

    return json;
});

app.MapFallbackToFile("index.html");

app.Run();


public record Clipboard(string Content, string Type);