using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP
{

    public class CommentsController : Controller
    {
        private readonly ICommments _context;
        public CommentsController(ICommments context)
        {
            this._context = context;
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
            if (comments==null)
            {
                return View();
            }

            comments.UserId = "de63b819-0429-430c-aa77-85a6126a527e";
            comments.EstateId = 7;
            _context.InsertComment(comments);
            
            return View();
        }

        public async Task<IActionResult> Details(long id)
        {
            if (id != null && id != 0)
            {
                Comments com = await _context.GetOne(id);
                return View(com);
            }
            return NotFound();


        }
    }
}
