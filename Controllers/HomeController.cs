﻿using GP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
        private readonly IlikedEstates _like;
        public HomeController(ILogger<HomeController> logger, 
            ICategory category,
            IType type,
            ICity city,
            IState state,
            IEstate estate,
             IlikedEstates like)
        {
            _logger = logger;
            _category = category;
            _type = type;
            _city = city;
            _state = state;
            _estate = estate;
            _like = like;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.FavEstate = _estate.GetAll().Where(z => z.is_active).OrderByDescending(x => x.Likes).Take(4).ToList();
            ViewBag.ModernEstate = _estate.GetAll().Where(z => z.is_active).OrderByDescending(x => x.OnDate).Take(8).ToList();
            ViewBag.ModernEstateApartment = _estate.GetAll().Where(z => z.is_active).Where(z => z.Category.category.Trim() == "شقة").OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.ModernEstateHouse = _estate.GetAll().Where(z => z.is_active && z.Category.category.Trim() == "منزل").OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.ModernEstateLand = _estate.GetAll().Where(z => z.is_active && z.Category.category.Trim() == "أرض").OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.ModernEstateChalet = _estate.GetAll().Where(z => z.is_active && z.Category.category.Trim() == "شاليه").OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.Likes = _like.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            return View();
        }

        public IActionResult FilterEstate(int Type,  int Category, int State,  long City)
        {
            ModelState.Remove("Type");
            ModelState.Remove("State");
            ModelState.Remove("City");
            ModelState.Remove("Category");
            if (!ModelState.IsValid) return View("Index");
            var data =  _estate.GetAll().Where(x => (Type == 0 ? true : x.TypeID == Type) &&
                                                    (Category == 0? true : x.categoryID == Category) &&
                                                    (State == 0? true: x.StateID == State) &&
                                                    (City == 0 ? true: x.CityID == City)&&
                                                    x.is_active
                                                    ).ToList();
            var Categ =  _category.GetOne(Category).Result;
            if(Categ is not null)
           ViewBag.CategoryName = Categ.category;
            return View(data);
        }

        public IActionResult Apartment()
        {
            ViewBag.Categories = _category.GetAll().ToList() ;
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            var data = _estate.GetAll().Where(x => x.Category.category.Trim() == "شقة").ToList();
            return View(data);
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
