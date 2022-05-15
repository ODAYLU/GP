using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            IQueryable<AppUser> users = _context.Users.Where(x => x.NameRole == "Owner").AsQueryable();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> GetProfile(string id)
        {
            await _user.FindByIdAsync(id);
            return View();

        }
         
    }
}
