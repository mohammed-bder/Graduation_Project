using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Patients
{
    public class PatientForProfileDto
    {
        [FullName]
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name must be at most 100 characters.")]
        public string FullName { get; set; } // Fname + Lname

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; } // Enum --> String (Readability)

        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of Birth must be in the past.")]
        public DateTime? DateOfBirth { get; set; }

        [Url(ErrorMessage = "Invalid picture URL format.")]
        public string? PictureUrl { get; set; }

        public string? BloodTrpe { get; set; }
    }
}
