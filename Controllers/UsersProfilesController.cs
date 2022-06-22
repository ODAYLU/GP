using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class UsersProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _user;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEstate _estates;
        private readonly IlikedEstates ilikedEstates;
        public UsersProfilesController(IEstate estates, 
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<AppUser> user,
            IlikedEstates ilikedEstates)
        {
            _estates = estates;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _user = user;
            this.ilikedEstates = ilikedEstates;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(string id)
        {
            AppUser user = await _user.FindByIdAsync(id);
            var IsOwner = await _user.IsInRoleAsync(user, "Owner");
            ViewBag.LikesEstate = ilikedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            if (IsOwner)
            {
                ViewData["flag"] = true;
                IEnumerable<Estate> lstEstates = (IEnumerable<Estate>)_estates.GetAll().Where(x => x.UserId == id && x.is_active && x.publish);
                ViewData["lstEstates"] = lstEstates;
            }
            return View(user);

        }



    }
}
