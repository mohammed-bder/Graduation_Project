using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
