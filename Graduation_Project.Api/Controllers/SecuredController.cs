using Graduation_Project.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuredController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public SecuredController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("secured")] // api/Secured/secured
        public IActionResult Secured()
        {
            return Ok("Hello From Secured");
        }
        //[HttpPost("SendEmail")]
        //public async Task<IActionResult> SendEmail(string to, string body, string subject)
        //{
        //    //if(await _emailService.SendEmailAsync(to, subject, body))
        //    //{
        //    //    return Ok("Email sent successfully");
        //    //}
        //    //return BadRequest("An error occurred while sending the email");

        //}
    }
}
