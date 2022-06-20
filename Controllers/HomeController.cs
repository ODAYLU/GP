using GP.Hubs;
using GP.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHubContext<NotificationHub> _hub;
        private readonly SignInManager<AppUser> signInManager;
        private readonly INotification _notification;
        public HomeController(ILogger<HomeController> logger,
            ICategory category,
            IType type,
            ICity city,
            IState state,
            IEstate estate,
            IlikedEstates likedEstates,
            SignInManager<AppUser> signInManager,
           IHubContext<NotificationHub> hub,
            INotification notification

        )
        {
            _logger = logger;
            _category = category;
            _type = type;
            _city = city;
            _state = state;
            _estate = estate;
            _likedEstates = likedEstates;
            this.signInManager = signInManager;
            _hub = hub;
            _notification = notification;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.FavEstate = _estate.GetAll().Where(z => z.is_active && z.publish && !z.IsBlock).OrderByDescending(x => x.Likes).Take(4).ToList();
            ViewBag.ModernEstate = _estate.GetAll().Where(z => z.is_active && z.publish && !z.IsBlock).OrderByDescending(x => x.OnDate).Take(8).ToList();
            ViewBag.ModernEstateApartment = _estate.GetAll().Where(z => z.is_active && z.publish && !z.IsBlock).Where(z => z.Category.category.Trim() == "شقة").OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.ModernEstateHouse = _estate.GetAll().Where(z => z.is_active && z.publish && z.Category.category.Trim() == "منزل" && !z.IsBlock).OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.ModernEstateLand = _estate.GetAll().Where(z => z.is_active && z.publish && z.Category.category.Trim() == "أرض" && !z.IsBlock).OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.ModernEstateChalet = _estate.GetAll().Where(z => z.is_active && z.publish && z.Category.category.Trim() == "شاليه" && !z.IsBlock).OrderByDescending(x => x.OnDate).Take(4).ToList();
            ViewBag.Likes = _likedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            return View();
        }

        public IActionResult FilterEstate(int Type, int Category, int State, long City)
        {
            ModelState.Remove("Type");
            ModelState.Remove("State");
            ModelState.Remove("City");
            ModelState.Remove("Category");
            if (!ModelState.IsValid) return View("Index");
            var data = _estate.GetAll().Where(x => (Type == 0 ? true : x.TypeID == Type) &&
                            (Category == 0 ? true : x.categoryID == Category) &&
                            (State == 0 ? true : x.StateID == State) &&
                            (City == 0 ? true : x.CityID == City) &&
                            x.is_active
                            ).ToList();
            var Categ = _category.GetOne(Category).Result;
            if (Categ is not null)
                ViewBag.CategoryName = Categ.category;
            return View(data);
        }

        public IActionResult Apartment()
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.Likes = _likedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            var data = _estate.GetAll().Where(x => x.Category.category.Trim() == "شقة" && x.is_active && x.publish && !x.IsBlock).ToList();
            return View();
        }
        public IActionResult House(List<Estate> data)
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.Likes = _likedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            ViewBag.Data = data;
            return View();
        }



        public IActionResult Land(List<Estate> data)
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.Likes = _likedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            ViewBag.Data = data;
            return View();
        }
        public IActionResult Chalet(List<Estate> data)
        {
            ViewBag.Categories = _category.GetAll().ToList();
            ViewBag.Cities = _city.GetAll().ToList();
            ViewBag.States = _state.GetAll().ToList();
            ViewBag.Types = _type.GetAll().ToList();
            ViewBag.Likes = _likedEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
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

        [HttpGet]
        public async Task<IActionResult> AddLikes(long id)
        {


            if (!signInManager.IsSignedIn(User) && !User.IsInRole("Admin"))
                return Ok("false");
            if (id != 0)
            {
                var estate = await _estate.GetOne(id);
                var data = new likedEstates
                {
                    IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    IdEstate = estate.Id
                };
                var like = _likedEstates.GetAll().FirstOrDefault(x => (x.IdUser == data.IdUser) && (x.IdEstate == data.IdEstate));
                if (like == null)
                {
                    await _likedEstates.InsertObj(data);
                    estate.Likes++;
                    await _estate.UpdateEstate(estate);
                    int count = estate.Likes;
                    string status = "like";
                                       
                        Notification msg = new Notification
                        {
                            Text = $"{User.Identity.Name}تم تسجيل الاعجاب على عقارك بواسطة ",
                            Time = DateTime.Now,
                            ReciverId = estate.UserId,
                            SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                            Type = "action",
                            IsReaded = (ConnectedUser.IDs.Contains(estate.UserId) ? true : false)
                        };
                        await _notification.InsertNot(msg);

                        await _hub.Clients.User(estate.UserId).SendAsync("receiveNotification", msg);
                    
                
                    var JsonData = new { status, count };
                    return Ok(JsonData);
                }
                else
                {
                    await _likedEstates.DeleteObj(like.Id);
                    estate.Likes--;
                    await _estate.UpdateEstate(estate);
                    int count = estate.Likes;
                    string status = "dislike";
                    var JsonData = new { status, count };
                    return Ok(JsonData);
                }


            }
            return BadRequest();
        }

        public async Task<IActionResult> GetNotification()

        {
            var data = await _notification.GetAll()
                 .Where(x => !x.IsReaded &&
                 x.ReciverId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                 .ToListAsync();

            if (data is not null) return Ok(data);
            else return Ok(null);
        }

        public async Task<IActionResult> SetNotificationReaded()
        {
            var data = await _notification.GetAll().Where(x => !x.IsReaded &&
                 x.ReciverId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();
            foreach (var item in data)
            {
                item.IsReaded = true;
            }
           await _notification.UpdateNot(data);
            return Ok();
        }

    }
}
