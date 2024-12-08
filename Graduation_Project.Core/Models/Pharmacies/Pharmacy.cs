namespace Graduation_Project.Core.Models.Pharmacies
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pharmacy name is required.")]
        [StringLength(100, ErrorMessage = "Pharmacy name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longtude { get; set; }

        [Required(ErrorMessage = "At least one contact number is required.")]
        [MinLength(1, ErrorMessage = "You must provide at least one contact number.")]
        [MaxLength(5, ErrorMessage = "You can provide a maximum of 5 contact numbers.")]
        [Phone(ErrorMessage = "Please enter a valid contact number.")]
        public List<string> ContactNumber { get; set; }

        [Required(ErrorMessage = "Pharmacy license image is required.")]
        [DataType(DataType.Upload)]
        public byte[]? PharmacyLicenseImgData { get; set; }

        [NotMapped]
        [DataType(DataType.Upload, ErrorMessage = "Please upload a valid license image.")]
        public IFormFile? PharmacyLicenseFile { get; set; }

        // M-M relationship (Medicine <=> Pharmacies)
        public ICollection<MedicinePharmacy> MedicinePharmacies { get; set; }

        // 1-M relationship (Pharmacist <=> Pharmacy)
        [Required(ErrorMessage = "Pharmacist ID is required.")]
        public int PharmacistId { get; set; }
        public Pharmacist Pharmacist { get; set; }

        // 1-M relationship (Pharmacy <=> PharmacyOrders)
        public ICollection<PharmacyOrder> PharmacyOrders { get; set; }
    }
}
