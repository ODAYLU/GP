using GP.Models;
using Microsoft.AspNetCore.Mvc;
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

        public ContactUserController(IContact contact)
        {
            _contact = contact;
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
            MailAddress to = new MailAddress(contact.Email.Trim());
            MailAddress from = new MailAddress("aqaramlack123@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "وصلتنا رسالتك سوف نتواصل معك في أقرب وقت";
            message.Body = contact.Description;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 465)
            {
                Credentials = new NetworkCredential("aqaramlack123@gmail.com", "a@1234567"),
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true
            };

            client.Send(message);
            
            _contact.InsertContact(contact);
            return RedirectToAction(nameof(Index));
        }

    }
}
