using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace GP
{
	[Authorize(Roles = "Owner")]
	public class ContractController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IContract _services;
		private readonly IEstate _estate;

		public ContractController(UserManager<AppUser> userManager, IContract contract, IEstate estate)
		{
			this._userManager = userManager;
			this._services = contract;
			this._estate = estate;
		}


		[HttpGet]
		public IActionResult Index(int? page)
		{


			var pageNumber = page ?? 1;
			int pageSize = 3;
			string UserId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);
			SeedData.IsPserosalPhoto = false;
			IEnumerable<Contract> list = _services.GetAll().Where(x => x.UserId == UserId1).ToPagedList(pageNumber, pageSize);
			return View(list);
		}

		[HttpGet]
		public async Task<IActionResult> CreateContract(long id)
		{
			Estate estate = await _estate.GetOne(id);
			if (estate == null)
			{

				return View("/Views/NotAccess.cshtml");
			}

			string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!UserIdLogin.Equals(estate.UserId))
			{
				return View("/Views/NotAccess.cshtml");
			}
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
			IdEstate.Id = id;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateContract(Contract contract)
		{
			Estate estate = await _estate.GetOne(IdEstate.Id);

			contract.Id = 0;

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

				return View(contract);
			}


			if (estate.TypeID != 1)
			{
				if (contract.up_to_date < estate.OnDate)
				{
					ModelState.AddModelError("up_to_date", "التاريخ غير صالح ");
				}
			}
			contract.isDone = (int)Enum.SatateContract.nonactive;
			contract.SallerName = estate.name_owner;
			contract.Sallerphone_num = estate.phone_num;
			contract.Latitude = estate.Latitude;
			contract.Longitude = estate.Longitude;
			contract.category = estate.Category.category;
			contract.Type = estate.Type.type;
			contract.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			contract.IDEstet = estate.Id;


			estate.is_active = false;
			estate.publish = false;
			await _estate.UpdateEstate(estate);

			await _services.InsertContract(contract);



			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public async Task<IActionResult> EeEstate(long id)
		{
			var user = await _userManager.GetUserAsync(User);

			//id = 60;
			if (user == null) return NoContent();


			if (user.memory > 0)
			{
				user.memory--;
			}
			else
			{
				Toast.Message = "الرجاء التواصل مع الادارة لشحن رصيدك ";
				Toast.ShowTost = true;

				return RedirectToAction(nameof(Index));


			}

			Contract contract = await _services.GetOne(id);

			if (contract == null) return NoContent();
			long IdEstate = contract.IDEstet;
			Estate estate = await _estate.GetOne(IdEstate);

			if (estate == null) return NoContent();

			estate.is_active = true;
			estate.publish = true;
			await _estate.UpdateEstate(estate);

			// تم التفعيل 
			contract.isDone = (int)Enum.SatateContract.active;

			await _services.UpdateContract(contract);

			//string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);


			// return RedirectToRoute($"Estate/Details/{IdEstate}");

			return RedirectToAction("Detalis", "Estate", new { id = IdEstate });

			//return View();

		}



		[HttpGet]
		public async Task<IActionResult> Details(long id)
		{
			Contract obj = await _services.GetOne(id);

			if (obj == null) return View("/Views/NotAccess.cshtml");


			string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!UserIdLogin.Equals(obj.UserId))
			{
				return View("/Views/NotAccess.cshtml");
			}
			return View(obj);
		}




	}
}
