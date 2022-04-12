using GP.Models;
using GP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly IContact _contact;

        public ContactController(IContact contact)
        {
            _contact = contact;
        }

        [HttpPost]
        public IActionResult ReceiveEmail(ContactVM contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var con = new Contact
            {
                Description = contact.Description,
                Email = contact.Email,
                Id = contact.Id,
                IsReply = true,
                Name = contact.Name,
                Object = contact.Object,
                Phone = contact.Phone
            };
            _contact.UpdateContact(con);
            MailAddress to = new MailAddress(contact.Email.Trim());
            MailAddress from = new MailAddress("aqaramlack123@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Amlack";
            message.Body = contact.Msg;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 465)
            {
                Credentials = new NetworkCredential("aqaramlack123@gmail.com","a@1234567"),
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true
            };

            client.Send(message);
            
            return Ok("");
        }
    }
}
