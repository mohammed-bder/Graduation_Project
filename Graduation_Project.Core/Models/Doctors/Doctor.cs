namespace Graduation_Project.Core.Models.Doctors
{
    public class Doctor: Person
    {

       

        //[Required(ErrorMessage = "National ID is required.")]
        [StringLength(14, ErrorMessage = "National ID must be 14 characters.")]
        public string? NationalID { get; set; }


        //public string MedicalLicence { get; set; } // we want to talk about this

        //[Required(ErrorMessage = "Medical License is required.")]
     
        public string? MedicalLicensePictureUrl { get; set; }

        //[NotMapped]
        //public IFormFile? MedicalLicenseFile { get; set; }



        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double? Rating { get; set; } = 0.0;


        [Required(ErrorMessage = "Consultation fees are required.")]
        [Range(0.0, 10000.0, ErrorMessage = "Consultation fees must be a positive number.")]
        public decimal ConsultationFees { get; set; }

        public int? ExperianceYears { get; set; }

        /* ----------------- Relationships ----------------- */

        // (1 Doctor ==> 1 Education)
        public Education Education { get; set; }

        // (M Doctor ==> M Subspeciality)
        public ICollection<DoctorSubspeciality> DoctorSubspeciality { get; set; }

        // (M Doctor ==> 1 Specialty)
        public int? SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; } 

        // (1 Doctor ==> M NotificationRecipient)
        //public ICollection<NotificationRecipient> NotificationRecipients { get; set; }

        // (1 Doctor ==> M TherapySession)
        public ICollection<TherapySession> TherapySessions { get; set; }

        // (1 Doctor ==> 1 Clinic)
        //public ICollection<DoctorClinic> DoctorClincs { get; set; }
        public Clinic Clinic { get; set; }



        // (1 Doctor ==> M Appointment)
        public ICollection<Appointment> Appointments { get; set; }

        // (1 Doctor ==> M Favorite)
        public ICollection<Favorite> Favorites { get; set; }

        // (1 Doctor ==> M Feedback)
        public ICollection<Feedback> Feedbacks { get; set; }

        // (1 Doctor ==> M Prescription)
        public ICollection<Prescription> Prescriptions { get; set; }

        public ICollection<WorkSchedule> WorkSchedules { get; set; }


    }

}
