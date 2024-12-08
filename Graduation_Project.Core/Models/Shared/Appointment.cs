namespace Graduation_Project.Core.Models.Shared
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } // Appointment date

        [Required]
        public DateTime Time { get; set; } // Appointment time

        //[Required]
        public string? VideoLink { get; set; } // me

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Appointment status (e.g., Scheduled, Completed, Cancelled)

        // Foreign Key: Doctor
        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } // Navigation property for Doctor

        // Foreign Key: Patient
        [Required]
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property for Patient

        [Required]
        public int ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } // Navigation property for Clinic


    }
}
