using Graduation_Project.Core.IServices;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
namespace Graduation_Project.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // Extract the email settings from the appsettings.json file
                var SMTPServer = _configuration["EmailSettings:SMTPServer"];
                var SMTPPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
                var Username = _configuration["EmailSettings:Username"];
                var Password = _configuration["EmailSettings:Password"];
                var UseTLS = bool.Parse(_configuration["EmailSettings:UseTLS"]);
                var EmailFrom = _configuration["EmailSettings:EmailFrom"];
                var EmailDisplayName = _configuration["EmailSettings:EmailDisplayName"];

                // Create a new MimeMessage
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(EmailDisplayName , EmailFrom));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart("html") { Text = body };

                // Create and configure the SMTP client
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(SMTPServer, SMTPPort,SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(Username, Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}