namespace Graduation_Project.Core.Models.Shared
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; } // Primary key for the favorite entity

        [Required]
        public int PatientId { get; set; } // Foreign key referencing the Patient

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property for the Patient

        [Required]
        public int DoctorId { get; set; } // Foreign key referencing the Doctor

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } // Navigation property for the Doctor
    }
}
