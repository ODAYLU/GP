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
    public class CategoryOperationController : Controller
    {
        private readonly ICategory _services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryOperationController(Models.ICategory category, IWebHostEnvironment webHostEnvironment)
        {
            this._services = category;
            this.webHostEnvironment = webHostEnvironment;
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoeyVM data)
        {

            ModelState.Remove("ImagePath");
            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View();

            if (data.file == null)
            {
                ModelState.AddModelError("ImagePath", "مطلوب ادخال صورة للتصنيف");
                return View();
            }

            string webRootPath = webHostEnvironment.WebRootPath;
            string upload = webRootPath + @"/images/Category/";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(data.file.FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await data.file.CopyToAsync(fileStream);
            }
            data.ImagePath = "/images/Category/" + fileName + extension;
            Category model = new Category
            {
                category = data.category,
                ImagePath = data.ImagePath
            };

            await _services.InsertCategory(model);
            return Ok(model);
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit(int Id)
        //{
        //    if (Id != 0)
        //    {
        //        Models.Category category = await _services.GetOne(Id);
        //        if (category != null)
        //        {
        //            return View(category);
        //        }

        //    }

        //    return NotFound();
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(CategoeyVM data)
        {
            ModelState.Remove("ImagePath");
            if (!ModelState.IsValid) return View();


            Category Newpro = await _services.GetOne(data.Id);
            if (data.file != null)
            {
                // حذف الصورة القديمه من المجلد 
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + Newpro.ImagePath;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }
                // اضافة الصورة الجديده
                string upload = webRootPath + @"/images/Category/";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(data.file.FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await data.file.CopyToAsync(fileStream);
                }
                Newpro.ImagePath = "/images/Category/" + fileName + extension;
            }

            Newpro.Id = data.Id;
            Newpro.category = data.category;

            await _services.UpdateCategory(Newpro);

            return Ok(Newpro);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            
            if (id == null)
                return NotFound();
      
            Models.Category category = await _services.GetOne(int.Parse(id));

            // حذف الصورة القديمه من المجلد 
            string webRootPath = webHostEnvironment.WebRootPath;
            string oldfile = webRootPath +  category.ImagePath;
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }
            await _services.DeleteCategory(int.Parse(id));
            return Ok(category);
        }
    }
}
