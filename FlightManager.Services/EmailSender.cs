using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FlightManager.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string email;
        private readonly string username;
        private readonly string password;

        public IConfiguration Configuration { get; }

        public EmailSender([FromServices] IConfiguration configuration)
        {
            this.Configuration = configuration;
            email = Configuration.GetValue<string>("EmailSender:EmailAddress");
            username = Configuration.GetValue<string>("EmailSender:Username");
            password = Configuration.GetValue<string>("EmailSender:Password");

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var toAddress = new MailAddress(email);
            var fromAddress = new MailAddress(this.email, username);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password),
                Timeout = 20000
            };

            using var emailMessage = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = htmlMessage
            };
            await smtp.SendMailAsync(emailMessage);
        }
    }
}
