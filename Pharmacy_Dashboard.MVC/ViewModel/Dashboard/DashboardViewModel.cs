namespace Pharmacy_Dashboard.MVC.ViewModel.Dashboard
{
    // ViewModel to hold all dashboard data
    public class DashboardViewModel
    {
        public int TotalPendingOrders { get; set; }
        public int TotalOrders { get; set; } // Or maybe total sales value? Adjust as needed
        public decimal TotalProfit { get; set; }
        public int TotalLowStock { get; set; }
        public List<LowStockViewModel> lowStockList { get; set; }
        public List<PendingOrdersViewModel> pendingOrdersList { get; set; }



        // chart 
        public List<OrderChartPointViewModel> OrdersLast30Days { get; set; }
        public List<TopMedicineViewModel> Top5Medicines { get; set; }



        public decimal TodaysEarnings { get; set; }
        public decimal TodaysPurchase { get; set; }
        public decimal TodaysCash { get; set; }
        public decimal TodaysBank { get; set; }
        public decimal TodaysService { get; set; }
        // Add properties for chart data if not loading via AJAX
    }
}
