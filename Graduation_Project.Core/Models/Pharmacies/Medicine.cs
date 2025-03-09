namespace Graduation_Project.Core.Models.Pharmacies
{
    public class Medicine : BaseEntity
    {
        public string? Name_ar { get; set; }

        [Required(ErrorMessage = "The medicine name is required.")]
        public string Name_en { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.0, 10000.0, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [StringLength(600, ErrorMessage = "The active substance cannot exceed 100 characters.")]
        public string? ActiveSubstance { get; set; }

        [StringLength(50, ErrorMessage = "The dosage form cannot exceed 50 characters.")]
        public string DosageForm { get; set; }

        [StringLength(50, ErrorMessage = "The route cannot exceed 50 characters.")]
        public string? Route { get; set; }


        // M-M relationship (Medicine <=> PharmacyOrders)
        public ICollection<MedicinePharmacyOrder>? MedicinePharmacyOrders { get; set; }


        // M-M relationship (Medicine <=> Pharmacies)
        public ICollection<MedicinePharmacy>? MedicinePharmacies { get; set; }

        // M-M relationship (Medicine <=> Prescriptions)
        public ICollection<MedicinePrescription>? MedicinePrescriptions { get; set; }
    }
}
