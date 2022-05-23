using GP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Areas.Identity.Pages.Account.Manage
{
    public class PersonalUserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public PersonalUserModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,

              ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _logger = logger;
        }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Phone]
            [Display(Name = "رقم التواصل")]
            [Required(ErrorMessage = "الحقل مطلوب")]
            public string PhoneNumber { get; set; }

            [Phone]
            [Display(Name = "رقم الوتس آب")]
            [Required(ErrorMessage = "الحقل مطلوب")]
            public string ContactNumber { get; set; }
            [Required(ErrorMessage = "الحقل مطلوب")]
            [Display(Name = "الاسم الاول")]
            public string fristName { get; set; }
            [Display(Name = "الاسم الثاني ")]
            [Required(ErrorMessage = "الحقل مطلوب")]
            public string lastName { get; set; }

            [Display(Name = "ProfilePicture")]
            public byte[] ProfilePicture { get; set; }




        }
        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userNow = await _userManager.GetUserAsync(User);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                fristName = userNow.FirstName,
                lastName = userNow.LastName,
                ContactNumber = userNow.ContactNumber


            };
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);



            user.FirstName = Input.fristName;
            user.LastName = Input.lastName;

            user.ContactNumber = Input.ContactNumber;









            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                using (var datastrem = new MemoryStream())
                {
                    await file.CopyToAsync(datastrem);
                    user.ProfilePicture = datastrem.ToArray();
                }


            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "تم تحديث البيانات بنجاح ";
            return RedirectToPage();
        }
    }
}
