using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP.Controllers
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
            IEnumerable<Comments>lst=_context.GetAll();
            return View(lst);
        }

        public async Task<IActionResult> Details(long id)
        {
            if (id!=null&&id!=0)
            {
                Comments com = await _context.GetOne(id);
                return View(com);
            }
            return NotFound();


        }
    }
}
