using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GP.Areas.Identity.Pages.Account
{
    public class ForgotPasswordConfirmation : PageModel
    {
        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./AccessDenied");
        }
    }
}
