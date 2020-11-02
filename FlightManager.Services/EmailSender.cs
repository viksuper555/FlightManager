using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            username = Configuration.GetValue<string>("EmailSender:Name");
            password = Configuration.GetValue<string>("EmailSender:Password");

        }
        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var toAddress = new EmailAddress(email);
            var fromAddress = new EmailAddress(this.email, username);

            try
            {
                var apiKey = Configuration.GetValue<string>("EmailSender:APIKey");
                var client = new SendGridClient(apiKey);

                var emailMessage = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, htmlMessage, htmlMessage.ToString());
                var response = await client.SendEmailAsync(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
