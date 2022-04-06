using GP.Data;
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
        private readonly ApplicationDbContext _context;

        public UserOperationController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>  BlockUser(string id,string day)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            user.is_active = false;
            if(user is null)
            {
                return View();
            }
            
            var endDate =  DateTime.Now.AddDays(int.Parse(day));

          await  _userManager.SetLockoutEnabledAsync(user ,true);
          await  _userManager.SetLockoutEndDateAsync(user,endDate); 
          await  _userManager.UpdateAsync(user);
           await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> UnBlockUser(string id)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            user.is_active = true;
            if (user is null)
            {
                return View();
            }


            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now-TimeSpan.FromMinutes(1));
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
