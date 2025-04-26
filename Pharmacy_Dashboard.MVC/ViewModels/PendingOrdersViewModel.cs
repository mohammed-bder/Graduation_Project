using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModels
{
    public class PendingOrdersViewModel
    {
        public string PatientName { get; set; }
        public string? DeliveryAddress { get; set; }

        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // e.g., "Delivered", "Cancelled", "Pending"


    }
}
