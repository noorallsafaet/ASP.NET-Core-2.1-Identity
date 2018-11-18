using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ASPNETCoreIdentity.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private string apiKey = "API";
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
        public Task SendEmailContactAsync(string email, string subject, string message, string Firstname, string Lastname, string Company)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("admin@muse.com", "Muse Administrator"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress("Your Email"));
            client.SendEmailAsync(msg);
            ConfimingEmail(email, "Thank you for contacting Muse");


            return Task.CompletedTask;
        }

        void ConfimingEmail(string Email, string Subject)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("admin@muse.com", "Muse Administrator"),
                Subject = Subject,
                PlainTextContent = "Thank you contacting us!",
                HtmlContent = "Thank you contacting us!"
            };
            msg.AddTo(new EmailAddress(Email));
            client.SendEmailAsync(msg);

        }
    }
}
