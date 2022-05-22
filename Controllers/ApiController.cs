using GP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace GP.Controllers
{
    //DataTable
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IEstate _estate;
        private readonly ICategory _category;
        private readonly ICity _city;
        private readonly IState _state;
        private readonly IService _service;
        private readonly IType _type;
        private readonly IContact _contact;
        private readonly UserManager<AppUser> _user;
        private readonly ICurrency _currency;
        private readonly ICommments _commments;
        private readonly IAdvertisement _advertisement;

        public ApiController(IEstate estate, ICategory category, ICity city, IState state, ICurrency currency, ICommments commments
            , IService service, IType type, UserManager<AppUser> user, IAdvertisement advertisement)
        {
            _estate = estate;
            _category = category;
            _city = city;
            _state = state;
            _service = service;
            _type = type;
            _user = user;
            _currency = currency;
            _commments = commments;
            _advertisement = advertisement;
        }
        [HttpPost]
        public async Task<IActionResult> GetEstate()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            var estates = _estate.GetAllQuiers();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                estates = estates.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await estates.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = estates.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> GetUser()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<AppUser> users = _user.Users.Where(x => x.NameRole == "User").AsQueryable();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                users = users.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }

            users = users.Where(x => string.IsNullOrEmpty(search) ? true :
            x.FirstName.Contains(search) || x.LastName.Contains(search));
            var data = await users.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = users.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }
        [HttpPost]
        public async Task<IActionResult> GetOwners()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];


            IQueryable<AppUser> users = _user.Users.Where(x => x.NameRole == "Owner").AsQueryable();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                users = users.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }

            users = users.Where(x => string.IsNullOrEmpty(search) ? true :
            x.FirstName.Contains(search) || x.LastName.Contains(search) || x.UserName.Contains(search));
            var data = await users.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = users.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(users);
        }
        [HttpGet]
        public async Task<IActionResult> GetOwnersCards(string? search)
        {
            if (search != null)
            {
                
                IQueryable<AppUser> lst = _user.Users.Where(x => x.NameRole == "Owner" &&( x.FirstName.Contains(search.ToLower())|| x.LastName.Contains(search.ToLower()))).AsQueryable();
              //  var owners = JsonSerializer.Serialize(lst);
                return Ok(lst);
            }
            else
            {
                IQueryable<AppUser> users = _user.Users.Where(x => x.NameRole == "Owner").AsQueryable();
              //  var json = JsonSerializer.Serialize(users);
                return Ok(users);
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> GetCategory()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<Category> categories = _category.GetAll(search).AsQueryable();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                categories = categories.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await categories.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = categories.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }
        [HttpPost]
        public async Task<IActionResult> GetContact()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];
            IQueryable<Contact> contacts = _contact.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                contacts = contacts.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await contacts.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = contacts.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
            return Ok(jsonData);
        }
        [HttpPost]
        public async Task<IActionResult> GetCity()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<City> cities = _city.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                cities = cities.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await cities.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = cities.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrency()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<Currency> currencies = _currency.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                currencies = currencies.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await currencies.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = currencies.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> GetState()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<State> states = _state.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                states = states.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await states.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = states.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> GetService()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<Services> services = _service.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                services = services.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await services.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = services.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }
        [HttpPost]
        public async Task<IActionResult> GetType()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<GP.Models.Type> types = _type.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                types = types.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await types.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = types.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> GetComments()
        {
          AppUser user = await _user.GetUserAsync(User);


            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

           // IQueryable<Comments> comments = _commments.GetAll(search);
            IQueryable<Comments> comments = _commments.GetAll(search).Where(x => x.AppUser.Equals(user));


            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
                {
                    comments = comments.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
                }
            var data = await comments.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = comments.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }
        [HttpPost]
        public async Task<IActionResult> GetAdvertisement()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skipe = int.Parse(Request.Form["start"]);
            var search = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortDirecion = Request.Form["order[0][dir]"];

            IQueryable<Advertisement> Advertisement = await _advertisement.GetAll(search);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
            {
                Advertisement = Advertisement.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
            }
            var data = await Advertisement.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = Advertisement.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
            return Ok(jsonData);
        }



    }
}
