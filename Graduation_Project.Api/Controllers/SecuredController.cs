using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuredController : ControllerBase
    {
        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("secured")] // api/Secured/secured
        public IActionResult Secured()
        {
            return Ok("Hello From Secured");
        }
    }
}
