using GP.Data;
using GP.Hubs;
using GP.Models;
using GP.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class ContactUserController : Controller
    {
        private readonly IContact _contact;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly MyEmailService _emailService;

        public ContactUserController(IContact contact,UserManager<AppUser> userManager
            , ApplicationDbContext context
            , SignInManager<AppUser> signInManager, MyEmailService emailService)
        {
            _contact = contact;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
             await _contact.InsertContact(contact);
          

            await _emailService.SendEmailAsync(contact.Email.Trim(),"موقع أملاك" ,"وصلتنا رسالتك سوف نتواصل معك في أقرب وقت");


            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Chat()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserName = currentUser.Id;
            ViewBag.Users = await _userManager.Users.Where(x => x.is_active).ToListAsync();
            var message = await _context.Messages.ToListAsync();
            return View();
        }

        [Authorize]
        public async Task<IActionResult> GetMessages(string ReciverId)
        {
            List<Message> data;
            string SenderId = _userManager.GetUserId(User);
            var msg = _context.Messages.Where(x => !x.IsReaded).ToList();
            foreach (var message in msg)
            {
                message.IsReaded = true;
            }
            _context.Messages.UpdateRange(msg);
            _context.SaveChanges();
            if(SenderId == ReciverId)
            {
                 data = await _context.Messages.Where(x => x.ReceiverId == ReciverId &&  x.ReceiverId == x.UserId).ToListAsync();
            }
            else
            {
                data = await _context.Messages.Where(x =>
                        (x.ReceiverId == ReciverId || x.ReceiverId == SenderId)
                        && (x.UserId == ReciverId || x.UserId == SenderId) 
                        ).ToListAsync();
              
            }
            return Ok(data);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUsers(string text)
        {
            List<object> lstUser = new List<object>();
            var Ids = text.Split(',');
            var msgs = _context.Messages.Where(x => !x.IsReaded).ToList();
            foreach (var item in Ids)
            {
              var user  = await _userManager.FindByIdAsync(item);
                if(msgs.Select(z => z.UserId).Contains(user.Id))
                {
                    bool flag = false;
                    lstUser.Add(new {user, flag });
                }
                else
                {
                    bool flag = true;
                    lstUser.Add(new { user, flag });
                }
                
            }
            return Ok(lstUser);
        }
        public async Task<IActionResult> Create(Message message)
        {
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.UserId = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

    }
}
