using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Area("Admin")]
    public class EstateController : Controller
    {
        private readonly IEstate services;
        private readonly IService servicesList; 
        private readonly IPhotoEstate _photoservices;
        private readonly IService_Estate _service_Estate;
        private readonly IWebHostEnvironment webHostEnvironment;


        public EstateController(GP.Models.IEstate Services, IWebHostEnvironment webHostEnvironment, IPhotoEstate photoservices, IService_Estate service_Estate, IService servicesList)
        {
            services = Services;
            this.webHostEnvironment = webHostEnvironment;
            _photoservices = photoservices;
            _service_Estate = service_Estate;
            this.servicesList = servicesList;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Estate> list = services.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult CreateTemp()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTemp(Estate estate, IFormFile image_main, List<IFormFile> images)
        {
            ModelState.Remove(nameof(estate.Id));
            ModelState.Remove(nameof(estate.Main_photo));
            if (!ModelState.IsValid) return View(estate);
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


            if (estate.list.Count() > 0)
            {
                for (int i = 0; i < estate.list.Count(); i++)
                {
                    Service_Estate service = new Service_Estate();
                    service.EstateID = idPr;
                    service.ServiceID = int.Parse(estate.list[i].Trim());
                    await _service_Estate.InsertService_Estate(service);

                }
            }
            GP.Models.Toast.ShowTost = true;
            GP.Models.Toast.Message = "تم اضافة العقار بنجاح ";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Estate>> Details(long id)
        {

            if (id != 0)
            {
                Estate estate = await services.GetOne(id);
                return View(estate);
            }

            return NotFound();

        }


        [HttpGet]
        public async Task<ActionResult<Estate>> Edit(long id)
        {
            List<Services> list=  _service_Estate.GetALl(id).ToList();
            // ViewBag.ser=list;

          
            if (id != 0)
            {

                Estate estate = await services.GetOne(id);
                string [] vs = new string [list.Count];

                for (int i = 0; i < list.Count; i++)
                {
                   vs[i] = list[i].Id.ToString();
                }
                estate.list = vs;
                

                return View(estate);
            }

            return NotFound();
        }

        public async Task<IActionResult> ActiveEstate(long Id)
        {
         var data =   await  services.GetOne(Id);
            if(data is not null)
            {
                if (data.is_active)
                {
                    data.is_active = false;
                }
                else
                {
                    data.is_active = true;
                }
               
               await services.UpdateEstate(data);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<Estate>> Edit(Estate estate)
        {
            
            Estate old = await services.GetOne(estate.Id);

        
            if (estate != null)
            {
               
                    await _service_Estate.DeleteService_Estate(old.Id);


                if (estate.list.Count() > 0)
                {
                    for (int i = 0; i < estate.list.Count(); i++)
                    {
                        Service_Estate service = new Service_Estate();
                        service.EstateID = old.Id;
                        service.ServiceID = int.Parse(estate.list[i].Trim());

                        await _service_Estate.InsertService_Estate(service);


                    }
                }


                estate.Main_photo = old.Main_photo;
                estate.is_active= old.is_active;
                estate.is_spacial=old.is_spacial;
                await services.UpdateEstate(estate);

                GP.Models.Toast.Message = "تم التعديل بنجاح";
                GP.Models.Toast.ShowTost = true;

                return RedirectToAction("Details", estate);
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
        [HttpPost]
        public async Task<IActionResult> DeleteImage(long id)
        {

            if (id != 0)
            {
                PhotoEstate estate = await _photoservices.GetOne(id);
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + @"\images\Estate\" + estate.ImagePath;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }

                await _photoservices.DeletePhotoEstate(id);

                ViewBag.id = estate.IdEstate;
            }

            return View("ImageList");

        }
        [HttpPost]
        public async Task<IActionResult> EditImage(long id)
        {
            PhotoEstate Newpro = await _photoservices.GetOne(id);

            var file = HttpContext.Request.Form.Files;
            if (file.Count() > 0)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string oldfile = webRootPath + @"\images\Estate\" + Newpro.ImagePath;
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }


                string upload = webRootPath + @"\images\Estate\";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(file[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                Newpro.ImagePath = fileName + extension;

                await _photoservices.UpdatePhotoEstate(Newpro);

            }
            ViewBag.id = Newpro.IdEstate;

            return View("ImageList");

        }

        [HttpPost]
        public async Task<IActionResult> AddImage(long q)
        {

            PhotoEstate Newpro = new PhotoEstate();
            string webRootPath = webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count() > 0)
            {


                string upload = webRootPath + @"\images\Estate\";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(file[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                Newpro.ImagePath = fileName + extension;
                Newpro.IdEstate = q;
                await _photoservices.InsertPhotoEstate(Newpro);

                ViewBag.id = q;

               
            }
            return View("ImageList");

        }
    }
 }
