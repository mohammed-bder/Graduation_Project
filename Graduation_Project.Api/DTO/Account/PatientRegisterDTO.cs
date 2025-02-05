using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Account
{
    public class PatientRegisterDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[RegularExpression("(?=^.{6,10}$)(?=.*\\d)( ?=.* [a-z])( ?=.* [A-Z]) ( ?=.* [!@#$%^&amp ;* ()_+}{&quot ;:; '?/&gt; .&lt;, ])( ?!.* \\\\s) .* $\",")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }


    }
}
