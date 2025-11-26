using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace MvcTask3.Utilites
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("kh6157987@gmail.com", "fiix ssls mmqk odzf")
            };
            return client.SendMailAsync(
                new MailMessage(
                    from: "kh6157987@gmail.com",
                    to: email,
                    subject,
                    htmlMessage
                    )
                {
                    IsBodyHtml = true
                });
        }
    }
}
