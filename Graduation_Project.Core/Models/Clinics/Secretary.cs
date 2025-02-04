namespace Graduation_Project.Core.Models.Clinics
{
    public class Secretary: Person
    {

        //[ForeignKey("ApplicationUser")]
        //public string UserId { get; set; }
        //public ApplicationUser? ApplicationUser { get; set; }

        //[Required(ErrorMessage = "National ID is required.")]
        [StringLength(14, ErrorMessage = "National ID must be 14 characters.")]
        public string? NationalID { get; set; } // National ID of the pharmacist

        // Relationships
        public ICollection<Doctor> doctors { get; set; } // Navigation property: a secretary can work with multiple doctors
        public ICollection<ClincSecretary> clincSecretarys { get; set; } // Navigation property: a Secretary can Work On multiple clinic
        public ICollection<NotificationRecipient> notificationRecipients { get; set; } // Navigation property: a Secretary can Work On multiple clinic


    }
}

