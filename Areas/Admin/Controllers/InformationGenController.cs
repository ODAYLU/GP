using GP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InformationGenController : Controller
    {
        private readonly IInformationGen information;

        public InformationGenController(IInformationGen information)
        {
            this.information = information;
        }

        public IActionResult Index()
        {
            var data = information.GetOne();
            return View(data);
        }

        public IActionResult GetInformation()
        {
            var data = information.GetOne();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> SaveInformation(InformationGen informatiom)
        {
            //if (!ModelState.IsValid) return BadRequest();
            await information.UpdateInformationGen(informatiom);
            return RedirectToAction(nameof(Index));
        }
    }
}
