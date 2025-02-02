namespace Graduation_Project.Core.Models.Patients
{
    public class RadiologyReport : BaseEntity
    {

        [StringLength(500, ErrorMessage = "Diagnosis cannot be longer than 500 characters.")]
        public string? Diagnosis { get; set; }

        [Required(ErrorMessage = "Image data is required.")]
        public string? PictureUrl { get; set; }

        [StringLength(1000, ErrorMessage = "AI Analysis cannot be longer than 1000 characters.")]
        public string? AIAnalysis { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
