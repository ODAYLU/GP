using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP
{

    [Authorize(Roles = "Owner,User")]
    public class likedEstatesController : Controller
    {
        private readonly IlikedEstates _servcies;

        public likedEstatesController(IlikedEstates estates)
        {
            this._servcies = estates;
        }
       
        public async Task<IActionResult> Index()
        {
            SeedData.IsPserosalPhoto = false;

            string UserId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<likedEstates> list = _servcies.GetAll().Where(x => x.IdUser == UserId1&& x.Estate.publish ).ToList();
            return View(list);
           
        }
        [HttpGet]
        public async Task<IActionResult> Delete (long id)
        {
            var x = await _servcies.GetOne(id);

            if(x != null)
            {
               await _servcies.DeleteObj(id);
                string UserId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);

                List<likedEstates> list = _servcies.GetAll().Where(x => x.IdUser == UserId1 && x.Estate.publish).ToList();
                var count = "";
                if (list.Count() == 0)
                {
                    count = "no";
                } 
                var status = "success";
                var JsonData = new { status,count };
                return Ok(JsonData);
            }

            return BadRequest();
          
        }
    }
}
