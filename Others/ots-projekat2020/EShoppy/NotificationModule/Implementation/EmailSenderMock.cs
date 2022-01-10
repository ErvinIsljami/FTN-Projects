using EShoppy.NotificationModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.NotificationModule.Implementation
{
    public class EmailSenderMock : IEmailSender
    {
        public void SendEmail(string email, string subject, string text)
        {
            Console.WriteLine($"Title: {subject}, to: {email}, text: {text}");
        }
    }
}
