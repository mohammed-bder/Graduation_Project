namespace Graduation_Project.Api.DTO.Doctors
{
    public class EducationDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Degree is required.")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Institution name is required.")]
        [StringLength(100, ErrorMessage = "Institution name cannot exceed 100 characters.")]
        public string Institution { get; set; }

        [Required(ErrorMessage = "Graduation date is required.")]
        [DataType(DataType.Date)]
        public DateTime GraduationDate { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        public string Specialty { get; set; }

        /* ----------------- Relationships ----------------- */

        // (M Education ==> 1 Doctor)
        public int DoctorId { get; set; }
    }
}
