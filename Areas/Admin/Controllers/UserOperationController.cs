﻿using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserOperationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> signInManager;

        public UserOperationController(UserManager<AppUser> userManager, ApplicationDbContext context,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddToMemory(string id, string count)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            user.memory = int.Parse(count);
            if (user is null)
            {
                return View();
            }

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult>  BlockUser(string id)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            user.is_active = false;
            if(user is null)
            {
                return View();
            }
            var result = await _userManager.IsLockedOutAsync(user);
            if (!result)
            {
               var res =  await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.UpdateAsync(user);
                await signInManager.SignOutAsync();
                await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromDays(1000));
                if (res.Succeeded) return Ok(user);
               
            }
            return BadRequest(user);
        }

        [HttpGet]
        public async Task<IActionResult> UnBlockUser(string id)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            user.is_active = true;
            if (user is null)
            {
                return View();
            }
            await _userManager.UpdateAsync(user);
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now-TimeSpan.FromMinutes(1));
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
