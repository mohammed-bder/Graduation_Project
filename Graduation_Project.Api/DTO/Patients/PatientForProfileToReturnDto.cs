using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Patients
{
    public class PatientForProfileToReturnDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PictureUrl { get; set; }
        public BloodType? BloodType { get; set; }
    }
}
