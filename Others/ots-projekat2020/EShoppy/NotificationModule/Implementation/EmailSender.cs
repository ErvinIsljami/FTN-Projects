using EShoppy.NotificationModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.NotificationModule.Implementation
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(string email, string subject, string text)
        {   
            //null treba proveriti
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("ktijana399@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = text;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("ktijana399@gmail.com", "tijana123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
