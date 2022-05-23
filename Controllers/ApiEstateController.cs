using GP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEstateController : ControllerBase
    {
        private readonly IEstate _estate;

        public ApiEstateController(IEstate estate)
        {
            _estate = estate;
        }

        [HttpGet("GetApartments")]
        public async Task<IActionResult> GetApartments(int pageNumber)
        {
            var estates =  _estate.GetAllQuiers();
            var data = await estates.Where(x => x.Category.category.Trim() == "شقة" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock).Skip(pageNumber * 20).Take(20).ToListAsync();
            int total = estates.Count();
            decimal totalDecimal = total / data.Count;
            int pages = (int)Math.Floor(totalDecimal);
            var JsonData = new {pages,data };
            return Ok(JsonData);
        }
        [HttpGet("GetHouses")]
        public async Task<IActionResult> GetHouses(int pageNumber)
        {
            var data = await _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "منزل" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock).Skip(pageNumber * 20).Take(20).ToListAsync();
            return Ok(data);
        }
        [HttpGet("GetLands")]
        public async Task<IActionResult> GetLands(int pageNumber)
        {
            var data = await _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "أرض" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock).Skip(pageNumber * 20).Take(20).ToListAsync();
            return Ok(data);
        }
        [HttpGet("GetChalets")]
        public async Task<IActionResult> GetChalets (int pageNumber)
        {
            var data = await _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "أرض" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock).Skip(pageNumber * 20).Take(20).ToListAsync();
            return Ok(data);
        }
    }
}
