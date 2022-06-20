using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OpinionController : Controller
	{
		private readonly IOpinion services;

		public OpinionController(IOpinion opinion)
		{
			this.services = opinion;
		}



		[HttpPost]
		public async Task<IActionResult> Edit(int Id)
		{
			try
			{
				Opinion oldopinion = await services.GetOne(Id);
				if (oldopinion.is_active)
				{
					oldopinion.is_active = false;
				}

				else
				{
					oldopinion.is_active = true;
				}

				await services.UpdateOpinion(oldopinion);

				return Ok(oldopinion);

			}
			catch
			{
				return BadRequest();
			}

		}
	}
}
