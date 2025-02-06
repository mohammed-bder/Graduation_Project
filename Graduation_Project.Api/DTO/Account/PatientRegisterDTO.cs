using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Account
{
    public class PatientRegisterDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name should Contain at least 3 characters")] 
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,10}$",
            ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter," +
            " one uppercase letter, one digit and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }


    }
}
