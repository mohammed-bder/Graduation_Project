using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModel.Stock
{
    public class PharmacyStockEditViewModel
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        [Required(ErrorMessage = "The Quantity field is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be Positive Integer number")]
        public int Quantity { get; set; }

    }
}
