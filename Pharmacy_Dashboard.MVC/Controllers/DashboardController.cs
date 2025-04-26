using Graduation_Project.Core;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_Dashboard.MVC.ViewModels;

namespace Pharmacy_Dashboard.MVC.Controllers
{
 
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Inject your data service here (e.g., IPharmacyDataService)
        // private readonly IPharmacyDataService _dataService;
        // public DashboardController(IPharmacyDataService dataService) { _dataService = dataService; }

        public DashboardController(
            IUnitOfWork unitOfWork
            )
        {
            this._unitOfWork = unitOfWork;
        }


        // TODO: Auth this end point
        public async Task<IActionResult> Index()
        {
            // TODO: get pharmacy id of registered pharmacy

            var _orderRepo = _unitOfWork.Repository<PharmacyOrder>();

            // 1. get total Pending orders
            var pharmacyPendingOrderCountSpec = new PharmacyOrderSpecification(pharmacyID: 1 , isOnlyPending: true);
            var totalPendingOrderFromDb = await _orderRepo.GetCountAsync(pharmacyPendingOrderCountSpec);


            // 2. get total pharmacy orders
            var pharmacyOrderCountSpec = new PharmacyOrderSpecification(pharmacyID: 1 , isOnlyPending: false);
            var totalOrderFromDb =   await _orderRepo.GetCountAsync(pharmacyOrderCountSpec);


            // 3. get total profit
            var pharmacyTotalProfitSpec = new GetPharmacyTotalProfitSpecification(pharmacyID: 1 );
            var totalProfitFromDB = await _orderRepo.GetSumAsync(pharmacyTotalProfitSpec, po => po.TotalPrice);



            // 4. get total number of  out of stock
            var _pharmacyMedicineStockRepo =  _unitOfWork.Repository<PharmacyMedicineStock>();
            var pharmacyMedicineLowStockCountSpec = new PharmacyMedicineLowStockSpecification(pharmacyID: 1);
            var lowStockMedicineCountFromDB = await _pharmacyMedicineStockRepo.GetCountAsync(pharmacyMedicineLowStockCountSpec);



            // 5. get out of stock ======> list
            var pharmacyMedicineLowStockSpec = new PharmacyMedicineLowStockSpecification(pharmacyID: 1);
            var lowStockMedicineListFromDB = await _pharmacyMedicineStockRepo.GetFirstWithSpecAsync(pharmacyMedicineLowStockSpec , 8);

            
            // 6. get Pending orders ======> list
            var pharmacyPendingOrderSpec = new PharmacyOrderSpecification(pharmacyID: 1 , isOnlyPending: true);
            var pharmacyPendingOrderListFromDB =await _orderRepo.GetFirstWithSpecAsync(pharmacyPendingOrderSpec , 8);
            


            var lowstockListViewMode = new List<LowStockViewModel>();
            foreach( var item in lowStockMedicineListFromDB)
            {
                var lowstockViewMode = new LowStockViewModel
                {
                    Name_en = item.Medicine.Name_en,
                    ActiveSubstance = item.Medicine.ActiveSubstance,
                    Price = item.Medicine.Price,
                    Quantity = item.Quantity
                };
                lowstockListViewMode.Add(lowstockViewMode);
            }


            var pendingOrderListViewModel = new List<PendingOrdersViewModel>();
            foreach(var item in pharmacyPendingOrderListFromDB)
            {
                var pendingOrdersViewModel = new PendingOrdersViewModel
                {
                    PatientName =   $"{item.Patient.FirstName}  {item.Patient.LastName}",
                    DeliveryAddress = item.DeliveryAddress,
                  TotalPrice = item.TotalPrice,
                  Status = item.Status
                };
                pendingOrderListViewModel.Add(pendingOrdersViewModel);
            }




            // 7. get orders last 30 days

            var ordersGroupedByDaySpec = new OrdersLast30DaysSpecification(pharmacyID: 1);

            var ordersLast30DaysFromDB = await _orderRepo.GetAllWithSpecAsync(ordersGroupedByDaySpec);

            var ordersLast30Days =  ordersLast30DaysFromDB.GroupBy(order => order.OrderDate.Date).Select(g => new OrderChartPointViewModel
            {
                Date = g.Key,
                OrderCount = g.Count()
            }).OrderBy(x => x.Date).ToList();





            // 8. get top 5 medicines
            //var top5Medicines = await _orderRepo.GetTop5MedicinesAsync(pharmacyId: 1);
            var top5MedicineSpec = new Top5MedicineSpecification();

            var medicinePharmacyOrderList = await _unitOfWork.Repository<MedicinePharmacyOrder>().GetAllWithSpecAsync(top5MedicineSpec);
            var top5Medicines = medicinePharmacyOrderList.GroupBy(mpo => mpo.MedicineId).Select(
                g => new TopMedicineViewModel
                {
                    MedicineName = g.First().Medicine.Name_en,
                    QuantitySold = g.Sum(mpo => mpo.Quantity)
                }
                ).OrderByDescending(x => x.QuantitySold).Take(5)
                .ToList();





            var viewModel = new DashboardViewModel
            {
                TotalPendingOrders = totalPendingOrderFromDb,
                TotalOrders = totalOrderFromDb,
                TotalProfit = totalProfitFromDB,
                TotalLowStock = lowStockMedicineCountFromDB,
                lowStockList = lowstockListViewMode,
                pendingOrdersList = pendingOrderListViewModel,
                OrdersLast30Days = ordersLast30Days,
                Top5Medicines = top5Medicines

            };


            // Pass the ViewModel to the View
            return View(viewModel);
        }




       
    }
}
