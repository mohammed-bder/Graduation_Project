using Graduation_Project.Core.Enums;

namespace Pharmacy_Dashboard.MVC.ViewModels.OrderViewModels
{
    public class OrderPageParams
    {
        public int pageNumber { get; set; } = 1;
        public int pharmacyId { get; set; } = 1;
        public OrderStatus? orderStatusFilter { get; set; }
        public DateTime? dateFilter { get; set; }

    }
}
