namespace Graduation_Project.Core.Models.Pharmacies
{
    // M-M relationship (Medicine <=> Pharmacies)

    public class MedicinePharmacy
    {
        public int PharmacyId { get; set; }
        public ICollection<Pharmacy> Pharmacies { get; set; }

        public int MedicineId { get; set; }
        public ICollection<Medicine> Medicines { get; set; }

        public int Quantity { get; set; }

    }
}
