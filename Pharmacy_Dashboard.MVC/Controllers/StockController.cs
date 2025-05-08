using AutoMapper;
using Graduation_Project.Core;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Specifications.MedicineSpecifications;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pharmacy_Dashboard.MVC.ViewModel;

namespace Pharmacy_Dashboard.MVC.Controllers
{
    public class StockController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pharmacyId = 1;
            // Get all PharmacyMedicine records for a specific Pharmacy
            var stockList = await _unitOfWork.Repository<PharmacyMedicineStock>()
                .GetAllWithSpecAsync(new StockForPharmacyWithMedicineSpecification(pharmacyId));

            // Prepare a simple view model for the view
            var viewModel = _mapper.Map<IReadOnlyList<PharmacyStockViewModel>>(stockList);
            var viewModel = stockList.Select(pm => new PharmacyStockViewModel
            {
                Id = pm.Id,
                MedicineName = pm.Medicine.Name_en,
                Quantity = pm.Quantity,
                Price = pm.Medicine.Price
            }).ToList();

            return View(viewModel);

        }

        // http://localhost:5152/Stock/AddPharmacyStock
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // You can adjust this based on your setup

            var medicines = await _unitOfWork.Repository<Medicine>().GetAllAsync();
            if (medicines == null)
                medicines = new List<Medicine>(); // Prevent null reference
            ViewBag.MedicineList = new SelectList(medicines, "Id", "Name_en");

            // If you're assigning to a specific pharmacy, set this ID (could also come from auth context)
            var pharmacyId = 1; // Replace with actual logic if dynamic
            ViewBag.PharmacyId = pharmacyId;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchMedicines(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var medicines = await _unitOfWork.Repository<Medicine>()
                .GetAllWithSpecAsync(
                    new MedicineSpec(term, 20),
                    m => new { id = m.Id, text = m.Name_en }
                );

            return Json(medicines);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddPharmacyMedicineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "The Quantity field is required");
                return View(model);
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
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index");
        }

        // http://localhost:5152/PharmacyMedicineStock/Edit/16
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editedRecord = await _unitOfWork.Repository<PharmacyMedicineStock>()
                .GetWithSpecsAsync(new StockWithMedicineSpecification(id));

            var viewModel =  new PharmacyStockViewModel
            {
                MedicineName = editedRecord.Medicine.Name_en,
                Quantity = editedRecord.Quantity,
            };

            return View("Edit", viewModel);
        }

        // POST: /PharmacyMedicineStock/Edit/5
        [HttpPost]
        public async Task<IActionResult> SaveEdit(PharmacyStockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "The Quantity field is required");
                return View(model);
            }
            var stock = await _unitOfWork.Repository<PharmacyMedicineStock>().GetByConditionAsync(pm => pm.Id == model.Id);
            if (stock == null)
                return NotFound();

            stock.Quantity = model.Quantity;
            _unitOfWork.Repository<PharmacyMedicineStock>().Update(stock);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index");
        }

    }
}
