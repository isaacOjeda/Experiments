using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auth0Test01.Web.Pages
{
    [AllowAnonymous]
    public class SignedOutModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
