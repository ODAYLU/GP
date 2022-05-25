using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit;
using System;
using MimeKit;
using GP.Service;

namespace GP.Service
{
    public class MyEmailService
    {
        EmailSettings emailSettings = null;

        public MyEmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }

        public async Task SendEmailAsync(string toemail, string subject, string Htmlmessage)
        {
            try
            {
                MimeMessage message1 = new MimeMessage();
                MailboxAddress from = new MailboxAddress(emailSettings.EmailTitle, emailSettings.EmailID);
                message1.From.Add(from);

                MailboxAddress to = new MailboxAddress(toemail, toemail);

                message1.To.Add(to);

                message1.Subject = subject;


                BodyBuilder emailBody = new BodyBuilder();

                emailBody.HtmlBody = Htmlmessage;


                message1.Body = emailBody.ToMessageBody();

                SmtpClient smc = new SmtpClient();

                //فتح اتصال على السيرفر

                smc.ConnectAsync(emailSettings.Host, emailSettings.Port, emailSettings.SSL).Wait();

                //عملت لوجن على البريد
                await smc.AuthenticateAsync(emailSettings.EmailID, emailSettings.Password);

                // ارسال الرسالة
                await smc.SendAsync(message1);

                // تسجيل الخروج
                await smc.DisconnectAsync(true);
            }
            catch (Exception er)
            {
                throw er;
            }
        }


    }
}
