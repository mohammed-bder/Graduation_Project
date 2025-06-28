using Graduation_Project.Core.Enums;

namespace Pharmacy_Dashboard.MVC.ViewModel.OrderViewModels
{
    public class UpdatedOrderParams
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }

    }
}
