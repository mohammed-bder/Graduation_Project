namespace Graduation_Project.Core.Models.Patients
{
    public class MedicalCategory
    {
        [Key]
        public int Id { get; set; } // Primary key for the medical category

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } // Name of the medical category

        // Relationships
        public ICollection<MedicalHistory> MedicalHistories { get; set; } // Navigation property: a category can have multiple doctors
    }
}

