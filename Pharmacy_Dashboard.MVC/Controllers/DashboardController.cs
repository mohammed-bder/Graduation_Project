using Microsoft.AspNetCore.Mvc;

namespace Pharmacy_Dashboard.MVC.Controllers
{
    // Define a ViewModel to hold all dashboard data
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalSalesCount { get; set; } // Or maybe total sales value? Adjust as needed
        public decimal TotalProfit { get; set; }
        public int OutOfStockCount { get; set; }
        public List<ExpiringItemViewModel> ExpiringItems { get; set; }
        public List<RecentOrderViewModel> RecentOrders { get; set; }
        public decimal TodaysEarnings { get; set; }
        public decimal TodaysPurchase { get; set; }
        public decimal TodaysCash { get; set; }
        public decimal TodaysBank { get; set; }
        public decimal TodaysService { get; set; }
        // Add properties for chart data if not loading via AJAX
    }

    // Example nested ViewModels for table data
    public class ExpiringItemViewModel
    {
        public string MedicineName { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Quantity { get; set; }
        // Add properties for 'Chart' and 'Return' columns if they have data
    }

    public class RecentOrderViewModel
    {
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } // e.g., "Delivered", "Cancelled", "Pending"
        public decimal Price { get; set; }
    }



    public class DashboardController : Controller
    {
        // Inject your data service here (e.g., IPharmacyDataService)
        // private readonly IPharmacyDataService _dataService;
        // public DashboardController(IPharmacyDataService dataService) { _dataService = dataService; }


        public IActionResult Index()
        {
            // --- Fetch data from your service/database ---
            // Replace with actual data fetching logic
            var viewModel = new DashboardViewModel
            {
                TotalCustomers = 120, // _dataService.GetTotalCustomers(),
                TotalSalesCount = 234, // _dataService.GetTotalSalesCount(),
                TotalProfit = 456m, // _dataService.GetTotalProfit(),
                OutOfStockCount = 56, // _dataService.GetOutOfStockCount(),
                TodaysEarnings = 5098.00m,
                TodaysPurchase = 2500m,
                TodaysCash = 1500m,
                TodaysBank = 1000m,
                TodaysService = 98m,
                ExpiringItems = new List<ExpiringItemViewModel> // _dataService.GetExpiringItems(5)
                {
                    new ExpiringItemViewModel { MedicineName = "Doxycycline", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Abetis", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Diasulin 10ml", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Cerox CV", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Fluclox", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Fluclox", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Fluclox", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Fluclox", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Fluclox", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 },
                    new ExpiringItemViewModel { MedicineName = "Fluclox", ExpireDate = new DateTime(2021, 12, 24), Quantity = 40 }
                },
                RecentOrders = new List<RecentOrderViewModel> // _dataService.GetRecentOrders(5)
                {
                    new RecentOrderViewModel { MedicineName = "Paricel 15mg", BatchNo = "783627834", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "88832433", Quantity = 40, Status = "Cancelled", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Cerox CV", BatchNo = "767676344", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Abetis 20mg", BatchNo = "45578866", Quantity = 40, Status = "Delivered", Price = 23.00m },
                    new RecentOrderViewModel { MedicineName = "Cerox CV", BatchNo = "767676344", Quantity = 40, Status = "Delivered", Price = 23.00m }
                }
            };

            // Pass the ViewModel to the View
            return View(viewModel);
        }

        // --- Optional: Actions for AJAX Chart Data ---

        [HttpGet]
        public JsonResult GetMonthlyProgressData(/* Potential parameters like year */)
        {
            // Fetch data from your service/database
            var data = new
            {
                labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                values = new[] { 65, 59, 80, 81, 56, 55, 40, 60, 90, 70, 75, 68 } // Replace with actual data
            };
            return Json(data);
        }

        [HttpGet]
        public JsonResult GetTodaysReportData()
        {
            // Fetch data from your service/database
            var data = new
            {
                labels = new[] { "Total Purchase", "Cash Received", "Bank Receive", "Total Service" },
                values = new[] { 2500, 1500, 1000, 98 } // Replace with actual data
            };
            return Json(data);
        }
    }
}
