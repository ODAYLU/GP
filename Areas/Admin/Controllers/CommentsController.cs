using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentsController : Controller
    {
        private readonly ICommments _context;
        private readonly IReplaies _replaies;
        private readonly IEstate _estate;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CommentsController(ICommments context, IWebHostEnvironment webHostEnvironment, IReplaies replaies, IEstate estate)
        {
            this._context = context;
            this._estate = estate;
            this._replaies = replaies;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Comments> lst = _context.GetAll();
            return View(lst);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Comments comments)
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

            comments.UserId = "de63b819-0429-430c-aa77-85a6126a527e";
            comments.EstateId = 7;

            _context.InsertComment(comments);

            return RedirectToAction("/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Details(long id)
        {
            if (id != 0)
            {
                Comments com = await _context.GetOne(id);
                Estate data = await _estate.GetOne(com.EstateId);
                string image = data.Main_photo;
                // ViewBag.image = data.Main_photo;
                //     TempData["Image"] = data.Main_photo;
                //  var Data = JsonSerializer.Serialize(data);
                var replaies = await _replaies.GetCommentReplies(id);
                var dataRep = await replaies.ToListAsync();
                var recordsTotal = dataRep.Count();
                var jsonData = new { recordsFiltered = replaies, recordsTotal, data = dataRep };
                return Ok(jsonData);
            }
            else
                return NotFound();


        }

    }
}
