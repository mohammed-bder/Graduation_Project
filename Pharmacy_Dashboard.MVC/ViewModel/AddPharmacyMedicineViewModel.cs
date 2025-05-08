using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModel
{
    public class AddPharmacyMedicineViewModel
    {
        public int PharmacyId { get; set; }
        [Required(ErrorMessage = "Please select the Medicine")]
        public int MedicineId { get; set; }
        [Required(ErrorMessage = "The Quantity field is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be Positive Integer number")]
        public int Quantity { get; set; }
    }
}
