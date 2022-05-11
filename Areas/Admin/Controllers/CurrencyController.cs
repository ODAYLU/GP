﻿using GP.Models;
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
    public class  CurencyController : Controller
    {
        private readonly ICurrency _services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CurencyController(Models.ICurrency service, IWebHostEnvironment webHostEnvironment)
        {
            this._services = service;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Currency currency)
        {
            ModelState.Remove("Id");
            //currency.currency=
            if (!ModelState.IsValid) return View();
            await _services.InsertCurrency(currency);
            return Ok(currency);


           
        }



        [HttpPost]
        public async Task<IActionResult> Edit(VMCurrency VMcurrency)
        {
            if (!ModelState.IsValid) return View();

            Models.Currency Newpro =  _services.GetOne(VMcurrency.Id).Result;
            

            Newpro.Id =VMcurrency.Id;
            Newpro.currency = VMcurrency.currency;

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
