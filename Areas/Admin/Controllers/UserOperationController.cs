using GP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserOperationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserOperationController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>  BlockUser(string id,string day)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            if(user is null)
            {
                return View();
            }
            
            var endDate =  DateTime.Now.AddDays(int.Parse(day));

          await  _userManager.SetLockoutEnabledAsync(user ,true);
          await  _userManager.SetLockoutEndDateAsync(user,endDate);
            return View();
        }
    }
}
