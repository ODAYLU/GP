using GP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP
{
    public class CommmentsController : Controller
    {
        private readonly ICommments _context;
        private readonly IReplaies _replaies;
        private readonly IEstate _estate;
        private readonly IPhotoEstate _photoEstate;
        private readonly UserManager<AppUser> _users;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IService_Estate _service_Estate;
        public CommmentsController(ICommments context, IPhotoEstate photoEstate, IWebHostEnvironment webHostEnvironment, IReplaies replaies, IEstate estate, IService_Estate service_Estate)
        {
            this._context = context;
            this._estate = estate;
            this._replaies = replaies;
            this._webHostEnvironment = webHostEnvironment;
            this._service_Estate = service_Estate;
            this._photoEstate = photoEstate;
        }
        public IActionResult Commments()
        {


            SeedData.IsPserosalPhoto = false;

            return View();
        }
        public IActionResult Index()
        {
            SeedData.IsPserosalPhoto = false;
            IEnumerable<Comments> lst = _context.GetAll();
            return View(lst);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Comments comments)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (comments == null)
            {
                return View();
            }
            ModelState.Remove("Id");
            AppUser user = await _users.GetUserAsync(User);
            Comments newone = new Comments()
            {
                UserId = user.Id,
                EstateId = 7,
                Body = comments.Body,
                Rating = comments.Rating,
            };


            await _context.InsertComment(newone);

            return RedirectToAction("/Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            //  Comments com = await _context.GetOne(id);
            await _context.DeleteComment(id);
            return Ok();

        }
        [HttpPost]
        public async Task<IActionResult> Details(long id)
        {
            if (id != 0)
            {
                Comments com = await _context.GetOne(id);
                var replaies = await _replaies.GetCommentReplies(id);
                return Ok(replaies);
            }
            else
                return NotFound();

            //Estate data = await _estate.GetOne(com.EstateId);
            //string image = data.Main_photo;
            // ViewBag.image = data.Main_photo;
            //     TempData["Image"] = data.Main_photo;
            //  var Data = JsonSerializer.Serialize(data);
            ////var recordsTotal = dataRep.Count();
            ////var jsonData = new { recordsFiltered = replaies, recordsTotal, data = dataRep };
            //List<string> x = new List<string>();
            //foreach (var item in dataRep)
            //{
            //    x.Add($"{item.body},{item.CommentId},{image} ");
            //}
        }
        [HttpGet]
        public async Task<IActionResult> EstateComment(long id)
        {
            Estate es = await _estate.GetOne(id);
            if (es == null)
                return View("/Views/NotAccess.cshtml");
            //var services =  _service_Estate.GetALl(id).ToList();
            // List<string>lst=services.Select(s => s.Name).ToList();

            // ViewBag.services= services;
            //var Ephotos = _photoEstate.GetAllByEstate(id);
            //List<string> photosPaths = Ephotos.Select(ph=>ph.ImagePath).ToList();
            //ViewBag.photosPaths=photosPaths;
            es.Views = es.Views + 1;
            await _estate.UpdateEstate(es);
            return View(es);
        }


        public async Task<IActionResult> Add(int x, int y, int z)
        {
            var isAppened = "Done";

            var json = new { isAppened };

            return Ok(json);

        }
    }
}

