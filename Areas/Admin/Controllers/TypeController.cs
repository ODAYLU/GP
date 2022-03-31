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
    public class TypeController : Controller
    {
        private readonly IType _services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TypeController(Models.IType service, IWebHostEnvironment webHostEnvironment)
        {
            this._services = service;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(VMType VMType)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View();

            if (VMType == null)
            {
                ModelState.AddModelError("ImagePath", "مطلوب ادخال صورة للنوع");
                return View();
            }

            string webRootPath = webHostEnvironment.WebRootPath;
            string upload = webRootPath + "/images/Types/";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(VMType.file.FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await VMType.file.CopyToAsync(fileStream);
            }
            VMType.ImagePath = "/images/Types/" +fileName + extension;

            Models.Type model = new Models.Type();
            model.Id = VMType.Id;
            model.type = VMType.type;
            model.ImagePath = VMType.ImagePath;
            await _services.InsertType(model);

            return Ok(model);
        }

 

        [HttpPost]
        public async Task<IActionResult> Edit(VMType vMType)
        {
            ModelState.Remove("file");
            ModelState.Remove("ImagePath");
            if (!ModelState.IsValid) return View();

            Models.Type Newpro = await _services.GetOne(vMType.Id);
            if (vMType.file != null)
            {
                // حذف الصورة القديمه من المجلد 
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath  + Newpro.ImagePath;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }
                // اضافة الصورة الجديده
                string upload = webRootPath + "/images/Types/";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(vMType.file.FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await vMType.file.CopyToAsync(fileStream);
                }
                Newpro.ImagePath = "/images/Types/"+ fileName + extension;
            }

            Newpro.Id = vMType.Id;
            Newpro.type = vMType.type;

            await _services.UpdateType(Newpro);

            return Ok(Newpro);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id ==null)
                return NotFound();

            Models.Type type = await _services.GetOne(int.Parse(id));

            // حذف الصورة القديمه من المجلد 
            string webRootPath = webHostEnvironment.WebRootPath;
            string oldfile = webRootPath + @"\images\Types\" + type.ImagePath;
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }
            await _services.DeleteType(int.Parse(id));
            return Ok();
        }
    }
}
