using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Doctor
{
    public class SortingDoctorDto
    {
        public string ApplicationUserId { get; set; } // Link to Identity DB
        public string FullName { get; set; } 
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        //public string? Specialty { get; set; }
        public string? PictureUrl { get; set; }
        public double? Rating { get; set; }
        public decimal ConsultationFees { get; set; }
    }
}
