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
    public class ServiceController : Controller
    {
        private readonly IService _services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ServiceController(Models.IService service, IWebHostEnvironment webHostEnvironment)
        {
            this._services = service;
            this.webHostEnvironment = webHostEnvironment;
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(VMService vMService)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View();

            if (vMService.file == null)
            {
                ModelState.AddModelError("ImagePath", "مطلوب ادخال صورة ggo]lm");
                return View();
            }

            string webRootPath = webHostEnvironment.WebRootPath;
            string upload = webRootPath + @"/images/Services/";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(vMService.file.FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await vMService.file.CopyToAsync(fileStream);
            }
            vMService.ImagePath = "/images/Services/" + fileName + extension;
            Models.Services model = new Services();
            model.Id = vMService.Id;
            model.Name = vMService.Name;
            model.ImagePath = vMService.ImagePath;

            await _services.InsertServices(model);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VMService vMservice)
        {
            
            if (!ModelState.IsValid) return View();

            Models.Services Newpro = await _services.GetOne(vMservice.Id);
            if (vMservice.file != null)
            {
                // حذف الصورة القديمه من المجلد 
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + "/images/Services/" + Newpro.ImagePath;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }
                // اضافة الصورة الجديده
                
                string upload = webRootPath + "/images/Services/";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(vMservice.file.FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await vMservice.file.CopyToAsync(fileStream);
                }
                Newpro.ImagePath = "/images/Services/" + fileName + extension;
            }

            Newpro.Id = vMservice.Id;
            Newpro.Name = vMservice.Name;

            await _services.UpdateServices(Newpro);

            return Ok(Newpro);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            Models.Services service= await _services.GetOne(int.Parse(id));

            // حذف الصورة القديمه من المجلد 
            string webRootPath = webHostEnvironment.WebRootPath;
            string oldfile = webRootPath + @"\images\Services\" + service.ImagePath;
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }
            await _services.DeleteServices(int.Parse(id));
            return Ok(id);
        }
    }
}
