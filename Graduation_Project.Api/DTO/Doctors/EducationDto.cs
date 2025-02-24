using System.ComponentModel.DataAnnotations;
using Graduation_Project.Api.Attributes;

namespace Graduation_Project.Api.DTO.Doctors
{
    public class EducationDto
    {
        [Required(ErrorMessage = "Specialization is required.")]
        [ExistingId<Specialty>]
        public int? SpecializationId { get; set; }

        [Required(ErrorMessage = "Sub_Specilaities is required.")]
        [ExistingIds<SubSpecialities>]
        public int[]? Sub_Specilaities_IDs { get; set; }

        [Required(ErrorMessage = "Degree is required.")]
        [ValidEnumValue<DoctorDegree>]
        public DoctorDegree Degree { get; set; }

        [Required(ErrorMessage = "Institution name is required.")]
        [StringLength(100, ErrorMessage = "Institution name cannot exceed 100 characters.")]
        public string Institution { get; set; } 

        [Required(ErrorMessage = "Certifications is required.")]
        [StringLength(200, ErrorMessage = "Certifications cannot exceed 200 characters.")]
        public string Certifications { get; set; }

        [Required(ErrorMessage = "Fellowships is required.")]
        [StringLength(200, ErrorMessage = "Fellowships cannot exceed 200 characters.")]
        public string Fellowships { get; set; }

        [Range(0, 100, ErrorMessage = "ExperianceYears must be between 0 and 100 Years.")]
        public int? ExperianceYears { get; set; }

    }
}
