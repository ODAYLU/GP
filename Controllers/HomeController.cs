using GP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IEstate _estate;
        public HomeController(ILogger<HomeController> logger, 
            ICategory category,
            IType type,
            ICity city,
            IState state,
            IEstate estate)
        {
            _logger = logger;
            _category = category;
            _type = type;
            _city = city;
            _state = state;
            _estate = estate;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.FavEstate = _estate.GetAll().OrderByDescending(x => x.Likes).Take(4).ToList();
            ViewBag.ModernEstate = _estate.GetAll().OrderByDescending(x => x.OnDate).Take(8).ToList();
            return View();
        }

        public IActionResult FilterEstate(int Type, [Required] int Category, int State,  long City)
        {
            ModelState.Remove("Type");
            ModelState.Remove("State");
            ModelState.Remove("City");
            if (!ModelState.IsValid) return View("Index");
            var data =  _estate.GetAll().Where(x => 
            x.TypeID == Type ||
            x.categoryID== Category ||
            x.StateID == State ||
            x.CityID == City
            ).ToList();
            var Categ =  _category.GetOne(Category).Result;
            if(Categ != null)
            {
                if(Categ.category.Trim() == "شقة") return RedirectToAction("Apartment", data);
                if(Categ.category.Trim() == "منزل") return RedirectToAction("House", data);
                if(Categ.category.Trim() == "أرض") return RedirectToAction("Land", data);
                if(Categ.category.Trim() == "شاليه") return RedirectToAction("Chalet", data);
            }
            return View("",data);
        }

        public IActionResult Apartment(List<Estate> data)
        {
            ViewBag.Data = data;
            return View();
        }
        public IActionResult House(List<Estate> data)
        {
            ViewBag.Data = data;
            return View();
        }
        public IActionResult Land(List<Estate> data)
        {
            ViewBag.Data = data;
            return View();
        }
        public IActionResult Chalet(List<Estate> data)
        {
            ViewBag.Data = data;
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
