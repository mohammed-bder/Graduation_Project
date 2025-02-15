using System.ComponentModel.DataAnnotations;
using Graduation_Project.Api.Attributes;

namespace Graduation_Project.Api.DTO.Doctor
{
    public class DoctorForProfileDto
    {
        [FullName(ErrorMessage = "Full name must contain at least two words.")] // Fname + Lname
        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(maximumLength:100 , MinimumLength = 3 , ErrorMessage = "Full Name must be at least 3 characters and at most 100 characters.")]
        public string FullName { get; set; } 

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Consultation Fees are required.")]
        [Range(0, 10000, ErrorMessage = "Consultation Fees must be between 0 and 10,000.")]
        public decimal ConsultationFees { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public Gender Gender { get; set; } // Enum --> String (Readability)

        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of Birth must be in the past.")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "PictureUrl cannot exceed 500 characters.")]
        public string? PictureUrl { get; set; }

    }
}
