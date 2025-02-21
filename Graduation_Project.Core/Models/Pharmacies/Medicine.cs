namespace Graduation_Project.Core.Models.Pharmacies
{
    public class Medicine : BaseEntity
    {

        [Required(ErrorMessage = "The medicine name is required.")]
        [StringLength(100, ErrorMessage = "The medicine name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "The dosage form cannot exceed 50 characters.")]
        public string DosageForm { get; set; }

        [StringLength(100, ErrorMessage = "The active substance cannot exceed 100 characters.")]
        public string ActiveSubstance { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int Quantity { get; set; }



        // M-M relationship (Medicine <=> PharmacyOrders)
        public ICollection<MedicinePharmacyOrder>? MedicinePharmacyOrders { get; set; }


        // M-M relationship (Medicine <=> Pharmacies)
        public ICollection<MedicinePharmacy>? MedicinePharmacies { get; set; }

        // M-M relationship (Medicine <=> Prescriptions)
        public ICollection<MedicinePrescription>? MedicinePrescriptions { get; set; }
    }
}
