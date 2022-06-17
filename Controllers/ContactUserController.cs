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
using System.Security.Claims;
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

        public ContactUserController(IContact contact, UserManager<AppUser> userManager
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


            await _emailService.SendEmailAsync(contact.Email.Trim(), "موقع أملاك", "وصلتنا رسالتك سوف نتواصل معك في أقرب وقت");


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
           await _context.SaveChangesAsync();
            if (SenderId == ReciverId)
            {
                data = await _context.Messages.Where(x => x.ReceiverId == ReciverId && x.ReceiverId == x.UserId).ToListAsync();
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
            bool flag = false;
            text ??= "";
            var Ids = text.Split(',');
            var msgs = _context.Messages.Where(x => !x.IsReaded).ToList();
            foreach (var item in Ids)
            {
                var user = await _userManager.FindByIdAsync(item);
                if (user != null)
                {

                    if (msgs.Select(z => z.UserId).Contains(user.Id))
                    {
                         flag = false;
                        lstUser.Add(new { user, flag });
                    }
                    else
                    {
                         flag = true;
                        lstUser.Add(new { user, flag });
                    }

                     flag = false;
                    
                    lstUser.Add(new {user, flag });

                }


            }
            return Ok(lstUser);
        }
        public async Task<IActionResult> GetUsersContact()
        {

            var lstUser = new List<object>();
            var userlst = new List<AppUser>();
            bool IsActive = false;
            var users = await _context.Messages.Include(x => x.Receiver)
                .Where(z => z.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(s => s.ReceiverId).Distinct()
                .ToListAsync();
            var msgs = _context.Messages.Where(x => !x.IsReaded).ToList();
            foreach (var item in users)
            {
                var user = await _userManager.FindByIdAsync(item);

                if (msgs.Select(z => z.UserId).Distinct().Contains(user.Id) && !userlst.Contains(user))
                {
                    bool flag = false;
                    if (ConnectedUser.IDs.Contains(user.Id)) IsActive = true;
                    else IsActive = false;

                    userlst.Add(user);


                   


                    
                    lstUser.Add(new { user, flag,IsActive });
                   

                }
                else
                {


                    if (!userlst.Contains(user))
                    {
                        bool flag = true;
                        if (ConnectedUser.IDs.Contains(user.Id)) IsActive = true;
                        else IsActive = false;
                        lstUser.Add(new { user, flag , IsActive });

                        userlst.Add(user);
                    }

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
