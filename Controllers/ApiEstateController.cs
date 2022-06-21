using GP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEstateController : ControllerBase
    {
        private readonly IEstate _estate;
        private readonly IlikedEstates _likeEstates;
        public ApiEstateController(IEstate estate, IlikedEstates likeEstates)
        {
            _estate = estate;
            _likeEstates = likeEstates;
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
            if(data.Count() == 0)
            {
                data = await estates.Skip((pageNumber-1) * 20).Take(20).ToListAsync();
            }
            decimal total = estates.Count();
            decimal totalDecimal = total / 20;
            int pages = (int)Math.Ceiling(totalDecimal);
            var likes = _likeEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            var JsonData = new {pages,data ,likes};
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
            var likes = _likeEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            var JsonData = new { pages, data, likes };
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
            var likes = _likeEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            var JsonData = new { pages, data, likes };
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
            var likes = _likeEstates.GetAll().Where(x => x.IdUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(z => z.IdEstate).ToList();
            var JsonData = new { pages, data, likes };
            return Ok(JsonData);
        }
    }
}
