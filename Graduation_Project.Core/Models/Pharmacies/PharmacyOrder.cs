namespace Graduation_Project.Core.Models.Pharmacies
{
    public class PharmacyOrder : BaseEntity
    {
        public OrderStatus Status { get; set; }
        public string? PrescriptionPictureUrl { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliverDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? DeliveryAddress { get; set; }


        //[Required(ErrorMessage = "Prescription image is required.")]
        //[DataType(DataType.Upload, ErrorMessage = "Please upload a valid prescription image.")]
        //public byte[]? PrescriptionImgData { get; set; }

        //[NotMapped]
        //[DataType(DataType.Upload, ErrorMessage = "Please upload a valid Prescription image.")]
        //[FileExtensions(Extensions = "jpg,jpeg,png,pdf", ErrorMessage = "Please upload a valid image or PDF file.")]
        //public IFormFile? PrescriptionFile { get; set; }




        // 1-M relationship (Pharmacy <=> PharmacyOrders)
        [Required(ErrorMessage = "Pharmacy is required.")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        // 1-M relationship (Patient <=> PharmacyOrders)
        [Required(ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }


        // M-M relationship (Medicine <=> PharmacyOrders)
        public ICollection<MedicinePharmacyOrder>? MedicinePharmacyOrders { get; set; }
    }
}
