using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api;


public static class Extensions
{
    public const string SECRET_KEY = "This is my custom Secret key for authnetication";
    public const string ISSUER = "http://localhost:5000";

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        return services;
    }
}