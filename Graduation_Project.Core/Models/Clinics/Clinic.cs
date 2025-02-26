namespace Graduation_Project.Core.Models.Clinics
{
    public class Clinic : BaseEntity
    {
        public string Name { get; set; } // Name of the clinic
        public string? PictureUrl { get; set; }
        public string? LocationLink { get; set; } //  locationLink of the clinic
        public double Latitude { get; set; }  // Latitude coordinate
        public double Longitude { get; set; }  // Longitude coordinate

        public string? Address { get; set; }     //  Address or location of the clinic




        public ClinicType? Type { get; set; }
        // Relationships
        public int RegionId { get; set; }
        public Region Region { get; set; }


        // Required foreign key and navigation property
        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }  


        //public ICollection<DoctorClinic> DoctorClinics { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<ContactNumber>? ContactNumbers { get; set; } // Navigation property: a clinic can have multiple doctors
        //public ICollection<Appointment>? Appointments { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<TherapySession>? therapySessions { get; set; } // Navigation property: a clinic can have multiple doctors
        public ICollection<ClincSecretary>? clincSecretarys { get; set; } // Navigation property: a clinic can have multiple Secretary

    }
}
