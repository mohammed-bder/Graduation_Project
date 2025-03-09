namespace Graduation_Project.Core.Models.Pharmacies
{
    public class Pharmacist: Person
    {

        //[ForeignKey("ApplicationUser")]
        //public string UserId { get; set; }
        //public ApplicationUser? ApplicationUser { get; set; }

        //[Required(ErrorMessage = "National ID is required.")]
        [StringLength(14, ErrorMessage = "National ID must be 14 characters.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must contain exactly 14 digits.")]
        public string? NationalID { get; set; } // National ID of the pharmacist


        //[Required(ErrorMessage = "Pharmacist license image is required.")]
        [DataType(DataType.Upload)]
       
        public string? PharmacistLicensePictureUrl { get; set; }

    


        // 1-M relationship (Pharmacist <=> Pharmacy)
        public ICollection<Pharmacy> Pharmacies { get; set; } // Navigation property: a pharmacist can work at multiple pharmacies


        // 1-M relationship (Pharmacist <=> NotificationRecipients)
        //public ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }
}
