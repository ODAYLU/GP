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
        public async Task<IActionResult> Create(Currency currency)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View();
            await _services.InsertCurrency(currency);
            return Ok(currency);


           
        }



        [HttpPost]
        public async Task<IActionResult> Edit(Currency currency)
        {
            if (!ModelState.IsValid) return View();

            Models.Currency Newpro =  _services.GetOne(currency.Id).Result;
            

            Newpro.Id = currency.Id;
            Newpro.currency = currency.currency;

            await _services.UpdateCurrency(Newpro);

            return RedirectToAction("Index");
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
