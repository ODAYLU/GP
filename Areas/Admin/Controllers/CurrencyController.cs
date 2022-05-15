using GP.Models;
using GP.Models.ViewModels;
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
    public class  CurrencyController : Controller
    {
        private readonly ICurrency _services;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CurrencyController(ICurrency service, IWebHostEnvironment webHostEnvironment)
        {
            this._services = service;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string currencyName)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View();
            var currency = new Currency
            {
                currency = currencyName
            };
            await _services.InsertCurrency(currency);
            return Ok(currency);


           
        }



        [HttpPost]
        public async Task<IActionResult> Edit(string currency,int id)
        {
            if (!ModelState.IsValid) return View();

            Models.Currency Newpro =  _services.GetOne(id).Result;
            

            Newpro.Id = id;
            Newpro.currency = currency;

            await _services.UpdateCurrency(Newpro);

            return Ok(Newpro);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            Models.Currency currency= await _services.GetOne(long.Parse(id));            
            await _services.DeleteCurrency(long.Parse(id));
            return Ok("Index");
        }
    }
}
