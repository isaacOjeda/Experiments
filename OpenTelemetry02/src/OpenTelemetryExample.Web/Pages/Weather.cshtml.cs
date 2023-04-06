using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenTelemetry.Web.Pages;

public class WeatherModel : PageModel
{
    private readonly ILogger<WeatherModel> _logger;
    private readonly HttpClient _http;

    public WeatherModel(ILogger<WeatherModel> logger, HttpClient http)
    {
        _logger = logger;
        _http = http;
    }

    public WeatherForecast WeatherData { get; set; } = new();

    public async Task OnGet()
    {
        WeatherData = await _http.GetFromJsonAsync<WeatherForecast>("http://localhost:5233/api/weather-forecast");
    }
}