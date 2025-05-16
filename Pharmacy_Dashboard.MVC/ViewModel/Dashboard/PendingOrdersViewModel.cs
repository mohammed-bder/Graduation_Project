using System.ComponentModel.DataAnnotations;
using Graduation_Project.Core.Enums;

namespace Pharmacy_Dashboard.MVC.ViewModel.Dashboard
{
    public class PendingOrdersViewModel
    {
        public string PatientName { get; set; }
        public string? DeliveryAddress { get; set; }

        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } // e.g., "Delivered", "Cancelled", "Pending"


    }
}
