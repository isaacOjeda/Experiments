using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auth0Test01.Web.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            var authenticationProperties = new AuthenticationProperties { RedirectUri = "/SignedOut" };

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
        }
    }
}
