namespace Graduation_Project.Core.Models.Patients
{
    public class MedicalCategory : BaseEntity
    {


        [Required(ErrorMessage = "Name is required.")]
        public string Name_ar { get; set; } 

        [Required(ErrorMessage = "Name is required.")]
        public string Name_en { get; set; } 

        // Relationships
        public ICollection<MedicalHistory>? MedicalHistories { get; set; } // Navigation property: a category can have multiple doctors
    }
}

