namespace Graduation_Project.Core.Models.Pharmacies
{
    public class Pharmacy : BaseEntity
    {
        public string ApplicationUserID { get; set; }


        [Required(ErrorMessage = "Pharmacy name is required.")]
        [StringLength(100, ErrorMessage = "Pharmacy name cannot exceed 100 characters.")]
        public string Name { get; set; }


        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }


        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longitude { get; set; }

        public string? Address { get; set; }


        [Required(ErrorMessage = "Pharmacy license Image is required.")]
        [DataType(DataType.Upload)]
        public string? LicenseImageUrl { get; set; }

        public string? ProfilePictureUrl { get; set; }


        [Required(ErrorMessage = "At least one contact number is required.")]
        [MinLength(1, ErrorMessage = "You must provide at least one contact number.")]
        [MaxLength(3, ErrorMessage = "You can provide a maximum of 3 contact numbers.")]
        [Phone(ErrorMessage = "Please enter a valid contact number.")]
        public ICollection<PharmacyContact> pharmacyContacts { get; set; }


        // M-M relationship (Medicine <=> Pharmacies)
        public ICollection<PharmacyMedicineStock>? pharmacyMedicineStocks { get; set; }

        // 1-M relationship (Pharmacy <=> PharmacyOrders)
        public ICollection<PharmacyOrder>? PharmacyOrders { get; set; }

    }
}
