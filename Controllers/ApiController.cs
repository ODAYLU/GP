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

namespace GP.Controllers
{
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
        private readonly UserManager<AppUser> _user;

        public ApiController(IEstate estate, ICategory category, ICity city, IState state
            , IService service, IType type, UserManager<AppUser> user)
        {
            _estate = estate;
            _category = category;
            _city = city;
            _state = state;
            _service = service;
            _type = type;
           _user = user;
        }
        //[HttpPost]
        //public async Task<IActionResult> GetEstate()
        //{
        //    var pageSize = int.Parse(Request.Form["length"]);
        //    var skipe = int.Parse(Request.Form["start"]);
        //    var search = Request.Form["search[value]"];
        //    var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
        //    var sortDirecion = Request.Form["order[0][dir]"];

        //    var estates =  _estate.GetAll();
        //    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDirecion)))
        //    {
        //        estates = estates.OrderBy(string.Concat(sortColumn, " ", sortDirecion));
        //    }
        //     var data = await estates.Skip(skipe).Take(pageSize).ToListAsync();
        //    var recordsTotal = estates.Count();
        //    var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

        //    return Ok(jsonData);
        //}

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
           
            users = users.Where(x => string.IsNullOrEmpty(search) ? true:
            x.FirstName.Contains(search)|| x.LastName.Contains(search));
            var data = await users.Skip(skipe).Take(pageSize).ToListAsync();
            var recordsTotal = users.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data};

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

            return Ok(jsonData);
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

        

    }
}
