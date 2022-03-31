using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly ICity _services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CityController(Models.ICity city, IWebHostEnvironment webHostEnvironment)
        {
            this._services = city;
            this.webHostEnvironment = webHostEnvironment;
        }



        [HttpPost]
        public async Task<IActionResult> Create(City city)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View();
            await _services.InsertCity(city);
            return Ok(city);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(City city)
        {
            if (!ModelState.IsValid) return View();


            City newcity = await _services.GetOne(city.Id);


            newcity.Id = city.Id;
            newcity.name = city.name;

            await _services.UpdateCity(newcity);

            return Ok(city);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            Models.City city = await _services.GetOne(int.Parse(id));
            

            await _services.DeleteCity(int.Parse(id));
            return Ok(city);
        }

    }
}


