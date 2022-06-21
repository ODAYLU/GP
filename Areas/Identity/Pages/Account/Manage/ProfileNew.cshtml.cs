using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GP.Areas.Identity.Pages.Account.Manage
{


    public class ProfileNewModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./AccessDenied");
        }


    }
}
