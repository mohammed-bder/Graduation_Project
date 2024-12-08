namespace Graduation_Project.Api.DTO.Account
{
    public class RoleDTO
    {
        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; }
    }
}
