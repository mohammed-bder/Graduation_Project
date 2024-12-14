namespace Graduation_Project.Core.Models.Clinics
{
    public class TherapySession : BaseEntity
    {
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


        // Foreign Key: Clinic 
        [Required]
        public int clinicId { get; set; } // Foreign key for Therapist (Doctor)

        [ForeignKey("clinicId")]
        public Clinic clinic { get; set; } // Navigation property for the associated therapist

        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }

}
