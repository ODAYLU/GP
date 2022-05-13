using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisement _services;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdvertisementController(IAdvertisement services, IWebHostEnvironment webHostEnvironment)
        {
            this._services = services;
            this._webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VMAdvertisement vmads)
        {
            ModelState.Remove("Id");
           // vmads.Photo = $"/images/Advertisement/{vmads.image}";
            if (!ModelState.IsValid) return View();

            if (vmads.image == null)
            {
                ModelState.AddModelError("ImagePath", "مطلوب ادخال صورة");
                return View();
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + @"\images\Advertisement\";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(vmads.image.FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await vmads.image.CopyToAsync(fileStream);
            }
            vmads.Photo = @"\images\Advertisement\" + fileName + extension;
            Advertisement model = new Advertisement();
            model.Id = vmads.Id;
            model.Photo = vmads.Photo;
            model.PhoneNumber = vmads.PhoneNumber;
            model.FirstName = vmads.FirstName;
            model.LastName = vmads.LastName;
            model.Email = vmads.Email;
            model.StartDate = vmads.StartDate;
            model.EndDate = vmads.EndDate;
            model.Link = vmads.Link;
            model.Description = vmads.Description;
            model.Price = vmads.Price;
            await _services.InsertAdvertisement(model);
           
            return Ok(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(VMAdvertisement ads)
        {
            
            if (!ModelState.IsValid) return View();
            Advertisement newads = await _services.GetOne(ads.Id);
            if (ads.image != null)
            {
                // حذف الصورة القديمه من المجلد 
                string webRootPath = _webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + @"\images\Advertisement\" + newads.Photo;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }
                // اضافة الصورة الجديده

                string upload = webRootPath + @"\images\Advertisement\";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(ads.image.FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await ads.image.CopyToAsync(fileStream);
                }
                newads.Photo = @"\images\Advertisement\" + fileName + extension;
            }

            newads.Id = ads.Id;
            newads.Photo = ads.Photo;
            newads.PhoneNumber = ads.PhoneNumber;
            newads.FirstName = ads.FirstName;
            newads.LastName = ads.LastName;
            newads.Email = ads.Email;
            newads.StartDate = ads.StartDate;
            newads.EndDate = ads.EndDate;
            newads.Link = ads.Link;
            newads.Description = ads.Description;
            newads.Price = ads.Price;

            await _services.UpdateAdvertisement(newads);

            return Ok(newads);



        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

           Advertisement ads = await _services.GetOne(int.Parse(id));

            // حذف الصورة القديمه من المجلد 
            string webRootPath = _webHostEnvironment.WebRootPath;
            string oldfile = webRootPath + @"\images\Advertisement\" + ads.Photo;
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }
            await _services.DeleteAdvertisement(int.Parse(id));
            return Ok(id);
        }
    }
}
