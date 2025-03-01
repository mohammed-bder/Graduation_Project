using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Patients
{
    public class PatientForProfileDto
    {
        [FullName(ErrorMessage = "Full name must contain at least two words.")] // Fname + Lname
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "Full Name must be at least 3 characters and at most 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of Birth must be in the past.")]
        public DateOnly? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "PictureUrl cannot exceed 500 characters.")]
        public string? PictureUrl { get; set; }

        [ValidEnumValue<BloodType>(ErrorMessage = "Invalid value for BloodTrpe.")]
        public BloodType? BloodTrpe { get; set; }
    }
}
