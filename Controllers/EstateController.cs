using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    public class EstateController : Controller
    {
        private readonly IEstate services;
        private readonly IPhotoEstate _photoservices;
        private readonly IService_Estate _service_Estate;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EstateController(GP.Models.IEstate Services, IWebHostEnvironment webHostEnvironment, IPhotoEstate photoservices, IService_Estate service_Estate)
        {
            services = Services;
            this.webHostEnvironment = webHostEnvironment;
            _photoservices = photoservices;
            _service_Estate = service_Estate;
        }

        [HttpGet]
        public IActionResult Index()
        {
           IEnumerable<Estate>  list = services.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Estate estate, IFormFile image_main, List<IFormFile> images, List<int> test)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string upload = webRootPath + @"\images\Estate\";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(image_main.FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await image_main.CopyToAsync(fileStream);
            }
            estate.Main_photo = fileName + extension;
            await services.InsertEstate(estate);


            long idPr = estate.Id;
            if (images.Count() > 0)
            {
               
                for (int i = 0; i < images.Count(); i++)
                {

                    PhotoEstate obj = new PhotoEstate();
                    fileName = Guid.NewGuid().ToString();
                    extension = Path.GetExtension(images[i].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        await images[i].CopyToAsync(fileStream);
                    }
                    obj.ImagePath = fileName + extension;

                    obj.IdEstate = idPr;
                    await _photoservices.InsertPhotoEstate(obj);
                }
            }


            if (test.Count > 0)
            {
                for (int i = 0; i < test.Count(); i++)
                {
                    Service_Estate service = new Service_Estate();
                    service.EstateID = idPr;
                    service.ServiceID=test[i];

                   await _service_Estate.InsertService_Estate(service);
                }
            }
            return RedirectToAction("Edit");
        }
        [HttpGet]
        public async Task<ActionResult<Estate>> Details(long id)
        {

            if(id != 0)
            {
                Estate estate = await services.GetOne(id);
                return View(estate);
            }

            return NotFound();

        }


        [HttpGet]
        public async Task<ActionResult<Estate>> Edit(long id)
        {

            //List<Services> list=  _service_Estate.GetALl(id).ToList();
            // ViewBag.ser=list;
          

            if (id != 0)
            {

                Estate estate = await services.GetOne(id);

                return View(estate);
            }

            return NotFound();
        }



        [HttpPost]
        public async Task<ActionResult<Estate>> Edit(Estate estate)
        {
            Estate old = await services.GetOne(estate.Id);

            // List<Services> list = _service_Estate.GetALl(id).ToList();
            if (estate != null)
            {
                estate.Main_photo = old.Main_photo;
                await  services.UpdateEstate(estate);

                GP.Models.Toast.Message = "تم التعديل بنجاح";
                GP.Models.Toast.ShowTost = true;

                return RedirectToAction("Details",estate);
            }

            return NotFound();
        }



        [HttpGet]
        public async Task<ActionResult<Estate>> Delete(long id)
        {

           
            if (id != 0)
            {
                Estate estate = await services.GetOne(id);
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + @"\images\Estate\" + estate.Main_photo;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }



                await services.DeleteEstate(id);




                GP.Models.Toast.Message = $" [ {id} ] تم حذف العقار  ";
                GP.Models.Toast.ShowTost = true;
                return RedirectToAction("Index");
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult ImageList(long id)
        {
            ViewBag.id = id;


            return View();
        }



    }
}
