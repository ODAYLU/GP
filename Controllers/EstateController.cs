namespace GP
{

	[Authorize(Roles = "Owner")]
	public class EstateController : Controller
	{
		private readonly ICommments _context;
		private readonly IReplaies _replaies;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEstate services;
		private readonly IService servicesList;
		private readonly IPhotoEstate _photoservices;
		private readonly IService_Estate _service_Estate;
		private readonly IlikedEstates _like;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly IHubContext<NotificationHub> _hub;
		private readonly INotification _notification;




		public EstateController(UserManager<AppUser> userManager,
					GP.Models.IEstate Services,
					IWebHostEnvironment webHostEnvironment,
					IPhotoEstate photoservices,
					IService_Estate service_Estate,
					IService servicesList,
					IlikedEstates like,
					IHubContext<NotificationHub> hub,
					INotification notification,
					ICommments _context,
					IReplaies _replaies
		 )
		{
			this._userManager = userManager;
			services = Services;
			this.webHostEnvironment = webHostEnvironment;
			_photoservices = photoservices;
			_service_Estate = service_Estate;
			this.servicesList = servicesList;
			_like = like;
			_hub = hub;
			_notification = notification;
			this._context = _context;
			this._replaies = _replaies;
		}

		//public bool publish { get; set; }

		[HttpPost]

		public IActionResult filterPost(int Estatecode, int status, int tYP, int cate, int? page)
		{
			var pageNumber = page ?? 1;
			int pageSize = 3;
			SeedData.IsPserosalPhoto = false;
			string UserId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);



			List<Estate> list2 = new();


			if (Estatecode != 0 || status != -1 || tYP != 0 || cate != 0)
			{
				if (Estatecode != 0)
				{
					list2.AddRange(services.GetAll().Where(x => x.UserId == UserId1 && x.is_active == true && x.Id == Convert.ToInt64(Estatecode)).ToList());


				}
				if (status != -1)
				{
					list2.AddRange(services.GetAll().Where(x => x.UserId == UserId1 && x.is_active == true && x.publish == Convert.ToBoolean(status)).ToList());


				}
				if (tYP != 0)
				{
					list2.AddRange(services.GetAll().Where(x => x.UserId == UserId1 && x.is_active == true && x.TypeID == tYP).ToList());


				}
				if (cate != 0)
				{
					list2.AddRange(services.GetAll().Where(x => x.UserId == UserId1 && x.is_active == true && x.categoryID == cate).ToList());
				}

				pageSize = list2.Count();
			}
			else
			{
				list2.AddRange(services.GetAll().Where(x => x.UserId == UserId1 && x.is_active == true).ToList());

				pageSize = 3;
			}


			IEnumerable<Estate> list;

			list = list2.ToPagedList(pageNumber, pageSize);
			return View(nameof(Index), list);
		}

		[HttpGet]
		public IActionResult Index(int? page)
		{

			var pageNumber = page ?? 1;
			int pageSize = 3;
			SeedData.IsPserosalPhoto = false;
			string UserId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var list = services.GetAll().Where(x => x.UserId == UserId1 && x.is_active == true).ToPagedList(pageNumber, pageSize);

			return View(list);
		}

		[HttpGet]
		public IActionResult GetEstate()
		{

			var data = services.GetAll();
			return Ok(data);
		}

		[HttpGet]
		public IActionResult CreateTemp()
		{

			SeedData.IsPserosalPhoto = false;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateTemp(Estate estate, IFormFile image_main, List<IFormFile> images)
		{
			SeedData.IsPserosalPhoto = false;
			if (image_main == null)
			{
				ModelState.AddModelError("image_main", "أضف الصورة الاساسية على الاقل");
				return View(estate);
			}
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
			var user = _userManager.GetUserAsync(User);
			estate.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

			if (estate.list != null)
			{


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

			}
			GP.Models.Toast.Message = $@" تم اضافة بنجاح  ( # {estate.Id} كود العقار ) ";
			GP.Models.Toast.ShowTost = true;

			return RedirectToAction("Index");
		}




		public async Task<ActionResult<Estate>> Detalis(long id)
		{


			SeedData.IsPserosalPhoto = false;

			if (id != 0)
			{
				Estate estate = await services.GetOne(id);
				if (estate == null)
				{
					return View("/Views/NotAccess.cshtml");
				}
				string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (!UserIdLogin.Equals(estate.UserId))
				{
					return View("/Views/NotAccess.cshtml");
				}
				return View(estate);
			}
			return View();

		}


		[HttpGet]
		public async Task<ActionResult<Estate>> Edit(long id)
		{

			if (id != 0)
			{
				Estate estate = await services.GetOne(id);

				if (estate != null)
				{
					string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
					if (!UserIdLogin.Equals(estate.UserId))
					{
						return View("/Views/NotAccess.cshtml");
					}


					SeedData.IsPserosalPhoto = false;
					List<Services> list = _service_Estate.GetALl(id).ToList();



					string[] vs = new string[list.Count];

					for (int i = 0; i < list.Count; i++)
					{
						vs[i] = list[i].Id.ToString();
					}
					estate.list = vs;


					return View(estate);
				}


			}

			return View("/Views/NotAccess.cshtml");
		}



		[HttpPost]
		public async Task<ActionResult<Estate>> Edit(Estate estate)
		{
			SeedData.IsPserosalPhoto = false;

			Estate old = await services.GetOne(estate.Id);


			if (estate != null)
			{

				await _service_Estate.DeleteService_Estate(old.Id);

				if (estate.list != null)
				{
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

				}

				estate.Main_photo = old.Main_photo;
				estate.is_active = old.is_active;
				estate.is_spacial = old.is_spacial;
				estate.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				await services.UpdateEstate(estate);

				GP.Models.Toast.Message = "تم التعديل بنجاح";
				GP.Models.Toast.ShowTost = true;

				return RedirectToAction("Detalis", estate);
			}
			return View("/Views/NotAccess.cshtml");
		}

		[HttpGet]
		public async Task<IActionResult> X(long id)
		{
			SeedData.IsPserosalPhoto = false;

			if (id != 0)
			{
				Estate estate = await services.GetOne(id);

				string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (!UserIdLogin.Equals(estate.UserId))
				{
					return View("/Views/NotAccess.cshtml");
				}
				if (estate.publish)
				{
					estate.publish = false;
				}

				else
				{
					estate.publish = true;
				}

				await services.UpdateEstate(estate);

				return RedirectToAction("Index");

			}

			return View("/Views/NotAccess.cshtml");
		}


		[HttpGet]
		public async Task<ActionResult<Estate>> Delete(long id)
		{
			SeedData.IsPserosalPhoto = false;
			if (id != 0)
			{
				Estate estate = await services.GetOne(id);
				if (estate != null)
				{
					string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
					if (!UserIdLogin.Equals(estate.UserId))
					{
						return View("/Views/NotAccess.cshtml");
					}
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

			}

			return View("/Views/NotAccess.cshtml");
		}


		[HttpGet]
		public async Task<IActionResult> ImageList(long id)
		{
			Estate estate = await services.GetOne(id);

			if (estate == null)
			{
				return View("/Views/NotAccess.cshtml");
			}
			if (estate != null)
			{
				string UserIdLogin = @User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (!UserIdLogin.Equals(estate.UserId))
				{
					return View("/Views/NotAccess.cshtml");
				}
			}

			SeedData.IsPserosalPhoto = false;
			if (id != 0)
			{
				ViewBag.id = id;

			}

			else
			{
				ViewBag.id = SeedData.EstateByImage;
			}
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> DeleteImage(long id)
		{
			SeedData.IsPserosalPhoto = false;
			long ids = 0;
			PhotoEstate estate = await _photoservices.GetOne(id);
			ids = estate.IdEstate;
			if (id != 0)
			{

				string webRootPath = webHostEnvironment.WebRootPath;
				string oldfile = webRootPath + @"\images\Estate\" + estate.ImagePath;
				if (System.IO.File.Exists(oldfile))
				{
					System.IO.File.Delete(oldfile);
				}

				await _photoservices.DeletePhotoEstate(id);

				ViewBag.id = estate.IdEstate;
			}

			SeedData.EstateByImage = ids;

			return RedirectToAction("ImageList", new { id = ids });

		}
		[HttpPost]
		public async Task<IActionResult> EditImage(long id)
		{
			SeedData.IsPserosalPhoto = false;
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
			SeedData.IsPserosalPhoto = false;

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
			SeedData.EstateByImage = q;
			return RedirectToAction("ImageList", new { id = q });

		}

		[HttpGet]
		public async Task<IActionResult> AddLikes(long id)
		{
			if (id != 0)
			{
				var estate = await services.GetOne(id);
				var data = new likedEstates
				{
					IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
					IdEstate = estate.Id
				};
				var like = _like.GetAll().FirstOrDefault(x => (x.IdUser == data.IdUser) && (x.IdEstate == data.IdEstate));
				if (like == null)
				{
					await _like.InsertObj(data);
					estate.Likes++;
					await services.UpdateEstate(estate);
					int count = estate.Likes;
					string status = "dislike";
					var JsonData = new { status, count };
					return Ok(JsonData);
				}
				else
				{
					await _like.DeleteObj(like.Id);
					estate.Likes--;
					await services.UpdateEstate(estate);
					int count = estate.Likes;
					string status = "dislike";
					var JsonData = new { status, count };
					return Ok(JsonData);
				}


			}
			return BadRequest();
		}

		[AllowAnonymous]
		public async Task<IActionResult> DetalesView()
		{


			var list = _context.GetAll().ToList();
			return View(list);
		}


		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> AddComment(long id, string body)
		{
			string status = "Insert";
			string name = "";
			string photo = "";
			long Id = -1;

			if (User.Identity.IsAuthenticated)
			{
				var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				name = user.FirstName + ' ' + user.LastName;

				if (user.ProfilePicture != null)
				{
					photo = "data:image/*;base64," + Convert.ToBase64String(user.ProfilePicture);

				}

				else
				{
					photo = "/indexLayout/assets/img/pages/user.jpg";
				}


				Comments comments = new Comments()
				{
					EstateId = id,
					Body = body,
					UserId = user.Id,
					Rating = 0

				};
				await _context.InsertComment(comments);
				var estate = await services.GetOne(Id);
				Notification msg = new Notification
				{
					Text = $"{User.Identity.Name}تم التعليق  على عقارك بواسطة ",
					Time = DateTime.Now,
					ReciverId = estate.UserId,
					SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
					Type = "comment",
					IsReaded = (ConnectedUser.IDs.Contains(estate.UserId) ? true : false)
				};
				await _notification.InsertNot(msg);

				await _hub.Clients.User(estate.UserId).SendAsync("receiveNotification", msg);
				Id = comments.Id;


				GP.Models.Toast.Message = "تم إضافة التعليق بنجاح";
				GP.Models.Toast.ShowTost = true;

			}

			else
			{
				status = "Disconnected";
			}



			var JsonData = new { status, name, photo, Id };
			return Ok(JsonData);


		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> AddReplay(long id, string body)
		{
			string status = "Insert";
			string name = "";

			string photo = "";
			if (User.Identity.IsAuthenticated)
			{
				var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				name = user.FirstName + ' ' + user.LastName;
				if (user.ProfilePicture != null)
				{
					photo = "data:image/*;base64," + Convert.ToBase64String(user.ProfilePicture);

				}

				else
				{
					photo = "/indexLayout/assets/img/pages/user.jpg";
				}



				Replaies replaies = new Replaies()
				{
					CommentId = id,
					body = body,
					UserId = user.Id,

				};

				await _replaies.InsertReply(replaies);
				var comment = await _context.GetOne(id);
				//Notification msg = new Notification
				//{
				//    Text = $"{User.Identity.Name}تم  الرد  على تعليقك بواسطة ",
				//    Time = DateTime.Now,
				//    ReciverId = comment.UserId,
				//    SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
				//    Type = "Reply",
				//    IdAction = $"{comment.EstateId}",
				//    IsReaded = (ConnectedUser.IDs.Contains(comment.UserId) ? true : false)
				//};
				//await _notification.InsertNot(msg);

				//await _hub.Clients.User(comment.UserId).SendAsync("receiveNotification", msg);

				//Notification msg2 = new Notification
				//{
				//    Text = $"{User.Identity.Name}تم  التعليق  على عقارك بواسطة ",
				//    Time = DateTime.Now,
				//    ReciverId = comment.Estate.UserId,
				//    SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
				//    Type = "comment",
				//    IsReaded = (ConnectedUser.IDs.Contains(comment.Estate.UserId) ? true : false)
				//};
				//await _notification.InsertNot(msg);

				//await _hub.Clients.User(comment.Estate.UserId).SendAsync("receiveNotification", msg2);
			}

			else
			{
				status = "Disconnected";
			}


			var JsonData = new { status, name, photo };
			return Ok(JsonData);


		}

	}
}
