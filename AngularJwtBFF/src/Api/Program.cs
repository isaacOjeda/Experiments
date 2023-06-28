using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


const string SECRET_KEY = "This is my custom Secret key for authnetication";
const string ISSUER = "http://localhost:5000";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();


// configura autenticación por Bearer Tokens, valida la expiración y el issuer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ISSUER,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/token", () =>
{
    // Genera un JWT dummy
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(SECRET_KEY);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim(ClaimTypes.Role, "Administrator")
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Issuer = ISSUER
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return new
    {
        token = tokenHandler.WriteToken(token)
    };
});


app.MapGet("/claims", (HttpContext http) =>
{
    var claims = http.User.Claims.Select(c => new { c.Type, c.Value });
    return claims;
}).RequireAuthorization();


// GET de productos dummies
app.MapGet("/products", () =>
{
    var products = new[]
    {
        new { Id = 1, Name = "Product 1" },
        new { Id = 2, Name = "Product 2" },
        new { Id = 3, Name = "Product 3" },
        new { Id = 4, Name = "Product 4" },
        new { Id = 5, Name = "Product 5" },
    };
    return products;
}).RequireAuthorization();

app.Run();
