using GP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GP.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {


        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [BindProperty]
        public List<IdentityRole> Roles { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "هذا الحقل مطلوب ")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "هذا الحقل مطلوب ")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "هذا الحقل مطلوب ")]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "يرجى إدخال الإيميل بشكل صحيح")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب ")]

            [DataType(DataType.Password)]
            [Display(Name = "Password")]

            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة السر")]
            [Compare("Password", ErrorMessage = "كلمتا السر غير متطابقتين")]
            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            public string ConfirmPassword { get; set; }

            public IFormFile Image { get; set; }

            public string Role { get; set; }
            public bool IsQwner { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();





            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();

                if (Input.IsQwner)
                {
                    Input.Role = "Owner";


                    user = new AppUser
                    {
                        UserName = new MailAddress(Input.Email).User,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        memory = 10,
                        NameRole = Input.Role,
                        is_active = true,

                        decription = "صاحب عقارات اقوم بتقديم خدمات البيع والايجار للعقارات من انواع مختلفة مع العديد من الخدمات  وبأسعار مناسبة للاشخاص المعنيين"



                    };

                }
                else
                {
                    Input.Role = "User";



                    user = new AppUser
                    {
                        UserName = new MailAddress(Input.Email).User,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        NameRole = Input.Role,
                        is_active = true,



                    };
                }

                var usernew = await _userManager.FindByNameAsync(user.UserName);

                if (usernew != null)
                {

                    ModelState.AddModelError(Input.Email, "المستخدم  موجود");
                    ViewData["Error"] = "المستخدم موجود";
                    return Page();

                }



                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.Role == "User")
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                    else if (Input.Role == "Owner")
                    {

                        await _userManager.AddToRoleAsync(user, "Owner");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }


            }



            return Page();
        }
    }
}