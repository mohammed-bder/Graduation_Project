using Graduation_Project.Api.DTO.Account;

namespace Graduation_Project.Api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO RoleFromRequest)
        {
            if (RoleFromRequest == null || string.IsNullOrWhiteSpace(RoleFromRequest.RoleName))
            {
                return BadRequest(new { Message = "Invalid data. Role name is required." });
            }

            // check role exist or not 
            var RoleExists = await roleManager.RoleExistsAsync(RoleFromRequest.RoleName);

            if (RoleExists)
            {
                return Conflict(new { Message = $"The role '{RoleFromRequest.RoleName}' already exists." });
            }

            IdentityRole NewRole = new IdentityRole
            {
                Name = RoleFromRequest.RoleName
            };
            var result = await roleManager.CreateAsync(NewRole);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Role Created Sucessfully" });
            }

            return StatusCode(500, new { Message = "Failed to create role.", result.Errors });
        }
    }
}
