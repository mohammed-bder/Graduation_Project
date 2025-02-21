using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Doctors
{
    public class SortingDoctorDto
    {
        public string ApplicationUserId { get; set; } // Link to Identity DB
        public string FullName { get; set; } 
        public decimal ConsultationFees { get; set; }
        public string? Specialty { get; set; }
        public string? PictureUrl { get; set; }
        public double? Rating { get; set; }
    }
}
