using GP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Area("Admin")]
    public class EstateController : Controller
    {
        private readonly IEstate services;
        public EstateController(GP.Models.IEstate Services)
        {
            services = Services;

        }

        public async Task<IActionResult> ActiveEstate(long Id)
        {
            var data = await services.GetOne(Id);
            if (data is not null)
            {
                if (data.IsBlock)
                {
                    data.IsBlock = false;
                }
                else
                {
                    data.IsBlock = true;
                }

                await services.UpdateEstate(data);
                return Ok(data.IsBlock);
            }
            return BadRequest();
        }
    }
}
