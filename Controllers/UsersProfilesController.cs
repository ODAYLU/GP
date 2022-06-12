using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class UsersProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _user;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersProfilesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> user)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _user = user;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
             return View();

        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(string id)
        {
           AppUser user=  await _user.FindByIdAsync(id);
            return View(user);

        }

        
         
    }
}
