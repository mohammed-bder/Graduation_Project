using Graduation_Project.Core;
using Graduation_Project.Core.Models.Pharmacies;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_Dashboard.MVC.ViewModel;

namespace Pharmacy_Dashboard.MVC.Controllers
{
    public class StockController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetPharmacystock(int pharmacyId)
        {
            // Get all PharmacyMedicine records for a specific Pharmacy
            var stockList = await _unitOfWork.Repository<PharmacyMedicineStock>()
                .GetManyByConditionAsync(pm => pm.PharmacyId == pharmacyId);

            // Prepare a simple view model for the view
            var viewModel = stockList.Select(pm => new PharmacyStockViewModel
            {
                MedicineName = pm.Medicine.Name_en,
                Quantity = pm.Quantity
            }).ToList();

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> AddPharmacyStock(AddPharmacyMedicineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if the Pharmacy already has this Medicine
            var existingRecord = await _unitOfWork.Repository<PharmacyMedicineStock>()
                .GetByConditionAsync(pm => pm.PharmacyId == model.PharmacyId && pm.MedicineId == model.MedicineId);

            if (existingRecord != null)
            {
                // Update the existing record
                existingRecord.Quantity += model.Quantity;
                _unitOfWork.Repository<PharmacyMedicineStock>().Update(existingRecord);
            }
            else
            {
                // Create a new record
                var newStock = new PharmacyMedicineStock
                {
                    PharmacyId = model.PharmacyId,
                    MedicineId = model.MedicineId,
                    Quantity = model.Quantity
                };
                await _unitOfWork.Repository<PharmacyMedicineStock>().AddAsync(newStock);
            }
            var result = await _unitOfWork.CompleteAsync();
            if (result == 0)
            {
                return BadRequest(new { message = "Failed to add medicine to Pharmacy." });
            }

            return Ok(new { message = "Medicine added to Pharmacy successfully!" });
        }
    }
}
