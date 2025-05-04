using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Account
{
    public class VerifyOtpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be exactly 6 digits.")]
        public string OtpCode { get; set; }
    }
}
