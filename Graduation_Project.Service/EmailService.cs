using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.SendingEmail;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
namespace Graduation_Project.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmailAsync(Email email)
        {
            try
            {
                // create the SMTP Client
                var smtpServer = _configuration["EmailSettings:SMTPServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
                var Username = _configuration["EmailSettings:Username"];
                var Password = _configuration["EmailSettings:Password"];
                var displayName = _configuration["EmailSettings:DisplayName"];

                // Create Email Message 
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(displayName, Username));
                mimeMessage.To.Add(MailboxAddress.Parse(email.Recipients));
                mimeMessage.Subject = email.Subject;
                mimeMessage.Body = new TextPart("html") { Text = email.Body };


                // Create and configure the SMTP client
                using var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(Username, Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}