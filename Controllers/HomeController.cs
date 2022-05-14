using GP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategory _category;
        private readonly IType _type;
        private readonly IState _state;
        private readonly ICity _city;
        public HomeController(ILogger<HomeController> logger, 
            ICategory category,
            IType type,
            ICity city,
            IState state)
        {
            _logger = logger;
            _category = category;
            _type = type;
            _city = city;
            _state = state;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            return View();
        }

        

        public IActionResult Privacy()
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
