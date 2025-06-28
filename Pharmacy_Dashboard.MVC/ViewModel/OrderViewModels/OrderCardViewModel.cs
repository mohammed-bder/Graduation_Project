using Graduation_Project.Core.Models.Pharmacies;

namespace Pharmacy_Dashboard.MVC.ViewModel.OrderViewModels
{
    public class OrderCardViewModel : OrderContent
    {
        public List<OrderMedicine> OrderMedicines { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public string? PatientPhone { get; set; }

    }
}
