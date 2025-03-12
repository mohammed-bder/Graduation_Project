using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Doctors
{
    public class DoctorForProfileToReturnDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public decimal ConsultationFees { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; } // Enum --> String (Readability)
        public DateOnly? DateOfBirth { get; set; }
        public string? PictureUrl { get; set; }
    }
}
