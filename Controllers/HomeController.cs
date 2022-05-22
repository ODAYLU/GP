using GP.Models;
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
        private readonly IlikedEstates _likedEstates;
        public HomeController(ILogger<HomeController> logger, 
            ICategory category,
            IType type,
            ICity city,
            IState state,
            IEstate estate,
            IlikedEstates likedEstates
            
        )
        {
            _logger = logger;
            _category = category;
            _type = type;
            _city = city;
            _state = state;
            _estate = estate;
            _likedEstates = likedEstates;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                SeedData.VsLikedEstate = _likedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(x => x.IdEstate).ToList();

            }
            List<long> vEstate = _estate.GetAll().Where(x => x.publish).Select(e => e.Id).ToList();
            //bool x =   vliked.Contains(2);



            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.FavEstate = _estate.GetAll().OrderByDescending(x => x.Likes).Where(x=>x.publish).Take(4).ToList();
            ViewBag.ModernEstate = _estate.GetAll().OrderByDescending(x => x.OnDate).Where(x => x.publish).Take(8).ToList();
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
        [HttpGet]
        public async Task<IActionResult> LikeEstateAndInserttheTable(long id)
        {
            Estate estate = await _estate.GetOne(id);
            if(estate is null)
            {
                return View("/Views/NotAccess.cshtml");
            }
            if (User.Identity.IsAuthenticated)
            {
                likedEstates likedEstates = new likedEstates()
                {
                    IdEstate = id,
                    IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };


                var list =  _likedEstates.GetAll().Where(e => e.IdEstate == estate.Id && e.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();

                if(list.Count == 0)
                {
                     await  _likedEstates.InsertObj(likedEstates);
                }

            }
            estate.Likes += 1;
            await _estate.UpdateEstate(estate);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> LikeEstateAndDetetetheTable(long id)
        {
            Estate estate = await _estate.GetOne(id);
            if (estate is null)
            {
                return View("/Views/NotAccess.cshtml");
            }
            if (User.Identity.IsAuthenticated)
            {
              

                   likedEstates likedEstates = _likedEstates.GetAll().Where(e => e.IdEstate == estate.Id && e.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();

             
                    await _likedEstates.DeleteObj(likedEstates.Id);
                

            }
            estate.Likes -= 1;
            await _estate.UpdateEstate(estate);
            return RedirectToAction("Index");
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
