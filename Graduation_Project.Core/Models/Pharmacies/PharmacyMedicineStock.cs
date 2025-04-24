namespace Graduation_Project.Core.Models.Pharmacies
{
    // M-M relationship (Medicine <=> Pharmacies)

    public class PharmacyMedicineStock : BaseEntity
    {
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int Quantity { get; set; }

    }
}
