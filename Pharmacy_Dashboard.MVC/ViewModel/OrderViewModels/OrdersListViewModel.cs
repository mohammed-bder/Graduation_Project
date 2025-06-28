namespace Pharmacy_Dashboard.MVC.ViewModel.OrderViewModels
{
    public class OrdersListViewModel
    {
        public List<OrderContent> Orders { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public OrderFilter CurrentFilter { get; set; }
    }
}
