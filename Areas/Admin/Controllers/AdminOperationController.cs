using GP.Hubs;
using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminOperationController : Controller
	{
		private readonly ILogger<AdminOperationController> _logger;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEstate _estate;
		private readonly IAdvertisement _advertisement;
		private readonly IContract _contract;

		public AdminOperationController(ILogger<AdminOperationController> logger,
            UserManager<AppUser> userManager,
            IEstate estate,
              IAdvertisement advertisement,
              IContract contract)
        {
            _logger = logger;
            _userManager = userManager;
            _estate = estate;
            _advertisement = advertisement;
            _contract = contract;
        }

        public IActionResult Index()
		{
			ViewBag.Active = ConnectedUser.IDs.Count();
			ViewBag.Users = _userManager.Users.Where(x => x.NameRole == "User").Count();
			ViewBag.Owner = _userManager.Users.Where(x => x.NameRole == "Owner").Count();
			ViewBag.Advertisement =  _advertisement.GetAll().ToList().Count();
			ViewBag.Estate = _estate.GetAll().Where(x => x.is_active).Count();
			ViewBag.Contract = _contract.GetAll().Count();
			return View();
		}
		public IActionResult User()
		{
			return View();
		}
		public IActionResult Owner()
        {
			return View();
        }
		public IActionResult Estate()
		{
			return View();
		}
		public IActionResult Category()
		{
			CategoeyVM categoey = new CategoeyVM();
			return View(categoey);
		}
		public IActionResult City()
		{
			return View();
		}
		public IActionResult State()
		{
			return View();
		}

		public IActionResult Service()
		{
			return View();
		}
		public IActionResult Type()
		{
			return View();
		}
		public IActionResult Currency()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
		//public IActionResult Comments()
		//{
		//	return View();
		//}
		public IActionResult Advertisement()
		{
			return View();
		}
		public IActionResult Owners()
		{
			return View();
		}

		public IActionResult Opinion()
		{
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
