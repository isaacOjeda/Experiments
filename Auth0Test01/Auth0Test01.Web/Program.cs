using Microsoft.IdentityModel.Protocols.OpenIdConnect;
/**
Refs:
https://auth0.com/blog/call-protected-api-in-aspnet-core/
https://community.auth0.com/t/why-is-my-access-token-not-a-jwt-opaque-token/31028
*/
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", options =>
{
    options.Cookie.Name = ".ClientWebAppAuth";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];

    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.ResponseType = OpenIdConnectResponseType.Code;
    
    options.SaveTokens = true;

    options.Events.OnRedirectToIdentityProvider = context =>
    {
        context.ProtocolMessage.SetParameter("audience", "https://Auth0Test01.Api");

        return Task.CompletedTask;
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages()
    .RequireAuthorization();

app.Run();
