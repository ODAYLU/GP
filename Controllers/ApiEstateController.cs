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
           if(pageNumber > 0) --pageNumber;
            var estates = _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "شقة" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock);
            var data = await estates.Skip(pageNumber * 20).Take(20).ToListAsync();
            decimal total = estates.Count();
            decimal totalDecimal = total / 20;
            int pages = (int)Math.Ceiling(totalDecimal);
            var JsonData = new {pages,data };
            return Ok(JsonData);
        }
        [HttpGet("GetHouses")]
        public async Task<IActionResult> GetHouses(int pageNumber)
        {
            if (pageNumber > 0) --pageNumber;
            var estates = _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "منزل" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock);
            var data = await estates.Skip(pageNumber * 20).Take(20).ToListAsync();
            decimal total = estates.Count();
            decimal totalDecimal = total / 20;
            int pages = (int)Math.Ceiling(totalDecimal);
            var JsonData = new { pages, data };
            return Ok(JsonData);
        }
        [HttpGet("GetLands")]
        public async Task<IActionResult> GetLands(int pageNumber)
        {
            if (pageNumber > 0) --pageNumber;
            var estates = _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "أرض" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock);
            var data = await estates.Skip(pageNumber * 20).Take(20).ToListAsync();
            decimal total = estates.Count();
            decimal totalDecimal = total / 20;
            int pages = (int)Math.Ceiling(totalDecimal);
            var JsonData = new { pages, data };
            return Ok(JsonData);
        }
        [HttpGet("GetChalets")]
        public async Task<IActionResult> GetChalets (int pageNumber)
        {
            if (pageNumber > 0) --pageNumber;
            var estates = _estate.GetAllQuiers().Where(x => x.Category.category.Trim() == "شاليه" &&
            x.is_active &&
            x.publish &&
            !x.IsBlock);
            var data = await estates.Skip(pageNumber * 20).Take(20).ToListAsync();
            decimal total = estates.Count();
            decimal totalDecimal = total / 20;
            int pages = (int)Math.Ceiling(totalDecimal);
            var JsonData = new { pages, data };
            return Ok(JsonData);
        }
    }
}
