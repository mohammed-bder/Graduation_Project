using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Account
{
    public class ResendOTP
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP Type is required.")]
        [EnumDataType(typeof(Core.Enums.OtpType), ErrorMessage = "Invalid OTP Type.")]
        public OtpType OtpType { get; set; }
    }
}
