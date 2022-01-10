using EShoppy.NotificationModule.Implementation;
using EShoppy.NotificationModule.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazaar.UnitTest.NotificationModule
{
    [TestFixture]
    public class EmailSenderTest
    {
        [Test]
        [TestCase("tijanakovacevic1998@gmail.com", "Naslov1", "ovo je tekst maila, ako mail stigne ovo ce da se ispise")]
        public void SendEmail_ValidTest(string email, string subject, string text)
        {
            IEmailSender emailSender = new EmailSender();

            try
            {
                emailSender.SendEmail(email, subject, text);
            }
            catch(Exception e)
            {

                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }
    }
}