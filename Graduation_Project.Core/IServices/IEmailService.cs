using Graduation_Project.Core.Models.SendingEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IEmailService
    {
        //Task<bool> SendEmailAsync(Email email);
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
