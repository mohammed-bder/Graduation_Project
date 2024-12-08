

namespace Graduation_Project.Core.Models.Clinics
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; } // Primary key for the clinic entity

        [Required(ErrorMessage ="Please ,Enter Your Name")]
        [StringLength(100,ErrorMessage ="Enter Shortest Name")]
        public string Name { get; set; } // Name of the clinic

        public byte[]? DbImgData { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }

        public string LocationLink { get; set; } //  locationLink of the clinic
        public string Location { get; set; }     //  Address or location of the clinic
        public ClinicType Type { get; set; }
        // Relationships
        public int RegionId { get; set; }
        [ForeignKey("RegionId")]
        public Region Region { get; set; }
        //public ICollection<DoctorClinic> DoctorClinics { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<ContactNumber> ContactNumbers { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<Appointment>? Appointments { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<TherapySession>? therapySessions { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<ClincSecretary> clincSecretarys { get; set; } // Navigation property: a clinic can have multiple Secretary

    }
}
