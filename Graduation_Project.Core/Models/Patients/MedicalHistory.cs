namespace Graduation_Project.Core.Models.Patients
{
    public class MedicalHistory : BaseEntity
    {

        [StringLength(500, ErrorMessage = "Details cannot exceed 500 characters.")]
        public string? Details { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("MedicalCategory")]
        public int MedicalCategoryId { get; set; }
        public MedicalCategory? MedicalCategory { get; set; }

        public MedicalHistoryImage? medicalHistoryImage { get; set; } // Navigation property: a medical history can have single image [1 to 1] relationship.
    }
}
