using Graduation_Project.Core.Enums;

namespace Pharmacy_Dashboard.MVC.ViewModel.OrderViewModels
{
    public class OrderFilter
    {
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? DateFilter { get; set; }
    }
}
