namespace Graduation_Project.Core.Models.Patients
{
    public class MedicalHistory
    {
        [Key]
        public int Id { get; set; }
        [StringLength(1000, ErrorMessage = "Details cannot be longer than 1000 characters.")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("MedicalCategory")]
        public int MedicalCategoryId { get; set; }
        public MedicalCategory MedicalCategory { get; set; }
    }
}
