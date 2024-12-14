namespace Graduation_Project.Core.Models.Patients
{
    public class AI_QuickDiagnosis : BaseEntity
    {

        [Required(ErrorMessage = "Symptoms are required.")]
        [StringLength(500, ErrorMessage = "Symptoms cannot be longer than 500 characters.")]
        public string Symptoms { get; set; }

        [StringLength(1000, ErrorMessage = "Recommendations cannot be longer than 1000 characters.")]
        public string? Recommendations { get; set; }

        [Required(ErrorMessage = "Created time is required.")]
        public DateTime CreatedTime { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
