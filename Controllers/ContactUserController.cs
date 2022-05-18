using GP.Data;
using GP.Hubs;
using GP.Models;
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

        public ContactUserController(IContact contact,UserManager<AppUser> userManager
            , ApplicationDbContext context
            , SignInManager<AppUser> signInManager)
        {
            _contact = contact;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendEmail(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _contact.InsertContact(contact);
            MailAddress to = new MailAddress(contact.Email.Trim());
            MailAddress from = new MailAddress("aqaramlack123@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "وصلتنا رسالتك سوف نتواصل معك في أقرب وقت";
            message.Body = contact.Description;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 465)
            {
                Credentials = new NetworkCredential("aqaramlack123@gmail.com", "a@1234567"),
                Port = 465,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            
            client.Send(message);

            
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
            List<AppUser> lstUser = new List<AppUser>();
            var Ids = text.Split(',');
            foreach (var item in Ids)
            {
              var user  = await _userManager.FindByIdAsync(item);
                lstUser.Add(user);
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
