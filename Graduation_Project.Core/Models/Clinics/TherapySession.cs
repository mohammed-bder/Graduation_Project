namespace Graduation_Project.Core.Models.Clinics
{
    public class TherapySession
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Session date is required. Please provide a valid session date.")]
        public DateTime SessionDate { get; set; }

        [Required(ErrorMessage = "Session time is required. Please specify the session time.")]
        public DateTime SessionTime { get; set; } 

        public string Location { get; set; } 

        public string Description { get; set; } 

        // Foreign Key: Patient
        [Required]
        public int PatientId { get; set; } // Foreign key for Patient

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property for the associated patient

        // Foreign Key: Therapist (Doctor or a specific Therapist entity)
        [Required]
        public int TherapistId { get; set; } // Foreign key for Therapist (Doctor)

        [ForeignKey("TherapistId")]
        public Doctor Therapist { get; set; } // Navigation property for the associated therapist

        // Foreign Key: Clinic 
        [Required]
        public int clinicId { get; set; } // Foreign key for Therapist (Doctor)

        [ForeignKey("clinicId")]
        public Clinic clinic { get; set; } // Navigation property for the associated therapist
    }

}
