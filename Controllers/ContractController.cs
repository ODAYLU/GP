using GP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContract _services;
        private readonly IEstate _estate;

        public ContractController(IContract contract,IEstate estate)
        {
            this._services = contract;
            this._estate = estate;
        }


        [HttpGet]
        public IActionResult Index()
        {

            IEnumerable<Contract> list = _services.GetAll();
            return View(list);
           
        }

        [HttpGet]
    
        public async Task<IActionResult> CreateContract(long id)
        
        {

            Estate estate = await  _estate.GetOne(id);
            if(estate == null) return NotFound();
            if(estate.TypeID == 1)
            {
                // 1 =>بيع

                ViewBag.TypeEstate = 1;
                ViewBag.TypeOfContract = "عقد بيع ";
            }

            else
            {
                ViewBag.TypeEstate = 0;
                ViewBag.TypeOfContract = "عقد إيجار ";
            }
            IdEstate.Id = id;
            
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> CreateContract(Contract contract)
        {
            Estate estate = await _estate.GetOne(IdEstate.Id);

            if (!ModelState.IsValid)
            {
                if (estate.TypeID == 1)
                {
                    // 1 =>بيع

                    ViewBag.TypeEstate = 1;
                    ViewBag.TypeOfContract = "عقد بيع ";
                }

                else
                {
                    ViewBag.TypeEstate = 0;
                    ViewBag.TypeOfContract = "عقد إيجار ";
                }
            }


            if(estate.TypeID != 1)
            {
               
                if(contract.up_to_date < estate.OnDate)
                {
                    ModelState.AddModelError("up_to_date", "التاريخ غير صالح ");
                }


            }
            contract.SallerName = estate.name_owner;
            contract.Sallerphone_num= estate.phone_num;
            contract.Latitude = estate.Latitude;
            contract.Longitude = estate.Longitude;
            contract.category = estate.Category.category;
            contract.Type = estate.Type.type;
            contract.UserId= User.FindFirstValue(ClaimTypes.NameIdentifier);


            estate.is_active = false;
            await  _estate.UpdateEstate(estate);

            await _services.InsertContract(contract);



            return View("Index");
        }



    }
}
