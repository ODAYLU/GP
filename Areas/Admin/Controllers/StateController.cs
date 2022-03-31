using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StateController : Controller
    {
        private readonly IState _services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public StateController(Models.IState state, IWebHostEnvironment webHostEnvironment)
        {
            this._services = state;
            this.webHostEnvironment = webHostEnvironment;
        }
       
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(VMState data)
        {
            ModelState.Remove("Id");
          
            if (!ModelState.IsValid)
            {
                Toast.ShowModel = true;
                return View();
            }
           
            if (data.file == null)
            {
                Toast.ShowModel = true;
                ModelState.AddModelError("ImagePath", "مطلوب ادخال صورة للدولة");
                return View("CreateAndEdit");
            }
            
          
            string webRootPath = webHostEnvironment.WebRootPath;
            string upload = webRootPath + @"/images/State/";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(data.file.FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await data.file.CopyToAsync(fileStream);
            }
            data.ImagePath = @"/images/State/" + fileName + extension;
            var state = new State
            {
                Id = data.Id,
                name = data.name,
                ImagePath = data.ImagePath
            };

            await _services.InsertState(state);
            return Ok(state);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(VMState data)
        {
            if (!ModelState.IsValid) {
                    Toast.ShowModel = true;
                    return View("CreateAndEdit");
                            }
            

            State Newpro = await _services.GetOne(data.Id);
            Toast.ShowModel = false;
            Toast.ShowTost = true;
            Toast.Message = $"Updated [{data.Id}] ";

            if (data.file != null)
            {
                // حذف الصورة القديمه من المجلد 
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + Newpro.ImagePath;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }
                // اضافة الصورة الجديدة
                string upload = webRootPath + @"/images/State/";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(data.file.FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await data.file.CopyToAsync(fileStream);
                }
                Newpro.ImagePath = fileName + extension;
                data.ImagePath = "/images/State/"+ Newpro.ImagePath;
            }


            var state = new State
            {
                Id = data.Id,
                name = data.name,
                ImagePath = data.ImagePath
            };

            await _services.UpdateState(state);

            return Ok(state);
        }
       [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            Models.State state = await _services.GetOne(id);

            // حذف الصورة القديمه من المجلد 
            string webRootPath = webHostEnvironment.WebRootPath;
            string oldfile = webRootPath  + state.ImagePath;
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }
            await _services.DeleteState(id);
            return RedirectToAction("CreateAndEdit");
        }
    }
}
