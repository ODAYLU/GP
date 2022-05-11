using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class AdminOperationController : Controller
    {
        private readonly ILogger<AdminOperationController> _logger;

        public AdminOperationController(ILogger<AdminOperationController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult User()
        {
            return View();
        }
        public IActionResult Category()
        {
            CategoeyVM categoey = new CategoeyVM();
            return View(categoey);
        }
        public IActionResult City()
        {
            return View();
        }
        public IActionResult State()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Type()
        {
            return View();
        }
         public IActionResult Currency()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }      
        public IActionResult Comments()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
