using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Account
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be exactly 6 digits.")]
        public string OTP { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,10}$",
            ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter," +
            " one uppercase letter, one digit and one special character.")]
        public string Password { get; set; }
    }
}
