using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InformationGenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformationGenController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.TInformatiomGensT.SingleOrDefault();
            return View(data);
        }

        public IActionResult GetInformation()
        {
           var data  = _context.TInformatiomGensT.SingleOrDefault();
            return Ok(data);
        }

        public IActionResult SaveInformation(InformationGen informatiom)
        {
            if (!ModelState.IsValid) return BadRequest();
           var data  = _context.TInformatiomGensT.ToList();
            _context.TInformatiomGensT.RemoveRange(data);
            _context.TInformatiomGensT.Add(informatiom);
            _context.SaveChanges();
            return Ok();
        }
    }
}
