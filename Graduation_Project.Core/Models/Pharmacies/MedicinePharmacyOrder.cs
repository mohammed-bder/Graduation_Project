namespace Graduation_Project.Core.Models.Pharmacies
{
    // M-M relationship (Medicine <=> PharmacyOrders)

    public class MedicinePharmacyOrder : BaseEntity
    {
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int PharmacyOrderId { get; set; }
        public PharmacyOrder PharmacyOrder { get; set; }

        public int Quantity { get; set; }
    }
}
