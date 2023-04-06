using OpenTelemetry.Api.Data;
using OpenTelemetry.Api.Extensions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTelemetryInstrumentation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSqlServer<ApiDbContext>(builder.Configuration.GetConnectionString("Default"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapPrometheusScrapingEndpoint();


app.MapGet("/api/weather-forecast", async (HttpClient http, ApiDbContext context) =>
{
    var url = "https://api.open-meteo.com/v1/forecast?latitude=28.68&longitude=-106.04&hourly=temperature_2m";

    var response = await http.GetAsync(url);

    response.EnsureSuccessStatusCode();

    var rawData = await response.Content.ReadAsStringAsync();

    var newLog = new WeatherLog
    {
        Date = DateTime.Now,
        RawData = rawData
    };

    context.WeatherLogs.Add(newLog);

    await context.SaveChangesAsync();

    var data = JsonSerializer.Deserialize<WeatherForecast>(rawData);

    return data;
});

app.Run();



