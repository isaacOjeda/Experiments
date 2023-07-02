using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {

        builderContext.AddRequestTransform(transformContext =>
        {
            var accessTokenClaim = transformContext.HttpContext.User.Claims
                 .FirstOrDefault(q => q.Type == "Access_Token");

            if (transformContext.HttpContext.User.Identity!.IsAuthenticated)
            {
                var claim = transformContext.HttpContext.User.Claims
                    .FirstOrDefault(q => q.Type == "Access_Token");

                if (accessTokenClaim != null)
                {
                    var accessToken = accessTokenClaim.Value;

                    transformContext.ProxyRequest.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", accessToken);
                }

            }

            return ValueTask.CompletedTask;
        });
    });


builder.Services
    .AddHttpClient()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AngularJWTBFF";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.MapPost("/local-login", async (
    LoginRequest request,
    HttpContext httpContext,
    IHttpClientFactory httpClientFactory) =>
 {
     var client = httpClientFactory.CreateClient();
     var baseAddress = app.Configuration["ApiHost:Url"];
     var response = await client.PostAsJsonAsync($"{baseAddress}/api/token", request);

     if (response.IsSuccessStatusCode)
     {
         var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

         var claims = new List<Claim>
            {
                new Claim("Access_Token", loginResponse.Token)
            };

         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
         var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

         await httpContext.SignInAsync(claimsPrincipal);

         // Leer el token y obtener los claims utilizando JWT
         var handler = new JwtSecurityTokenHandler();
         var token = handler.ReadJwtToken(loginResponse.Token);

         return Results.Ok(new
         {
            token.ValidTo,
            Name = token.Claims.Where(q => q.Type == "unique_name").FirstOrDefault()?.Value,
         });
     }

     return Results.Forbid();
 });

app.MapPost("local-logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync();
    return Results.Ok();
});

app.MapFallbackToFile("index.html");


app.Run();


public record LoginRequest(string UserName, string Password);
public record LoginResponse(string Token);