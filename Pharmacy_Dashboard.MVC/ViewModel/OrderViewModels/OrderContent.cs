using Graduation_Project.Core.Enums;

namespace Pharmacy_Dashboard.MVC.ViewModel.OrderViewModels
{
    public class OrderContent
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PatientName { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } // e.g., "Confirmed", "Pending"

    }
}
