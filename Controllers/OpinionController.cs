using GP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class OpinionController : Controller
    {
        private readonly IOpinion services;

        public OpinionController(IOpinion opinion)
        {
            this.services = opinion;
        }
        // GET: OpinionController
        public ActionResult Index()
        {
            return View();
        }
        // POST: OpinionController/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Opinion opinion, string radio1)
        {


            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Redirect("/Identity/Account/Login");
                }
                if (opinion == null)
                {
                    return View(nameof(Index));
                }
                opinion.Rating = Convert.ToInt32(radio1);
                opinion.IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await services.InsertOpinion(opinion);

            }
            catch (Exception ex)
            {

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
