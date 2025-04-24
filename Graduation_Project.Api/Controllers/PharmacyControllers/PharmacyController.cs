using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Specifications.MedicineSpecifications;
using Graduation_Project.Service.HelperModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.PharmacyControllers
{
    public class PharmacyController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPharmacyService _pharmacyService;

        public PharmacyController(IUnitOfWork unitOfWork, IPharmacyService pharmacyService)
        {
            _unitOfWork = unitOfWork;
            _pharmacyService = pharmacyService;
        }

        [HttpGet]
        public async Task<ActionResult<Pharmacy>> FindNearestPharmacies(PatientLocationWithMedicinesDto patientLocationWithMedicinesDto)
        {
            // Find Pharmacies that have the medicines 
            // 1: Get Pharmacies Ids from PharmacyMedicineStock (M == M) Table
            var pharamciesStockAvaliabilitySpecs = new PharamciesStockAvaliabilitySpecs(patientLocationWithMedicinesDto.Medicines);
            var pharmacyMedicineStocks = await _unitOfWork.Repository<PharmacyMedicineStock>().GetAllWithSpecAsync(pharamciesStockAvaliabilitySpecs);
            if(pharmacyMedicineStocks is null)
                return BadRequest(new ApiResponse(404));

            var distinctPharmaciesIds = pharmacyMedicineStocks
                .DistinctBy(p => p.PharmacyId)
                .Select(d => d.PharmacyId)
                .ToList();

            // 2: Get Pharmacies
            var pharmaciesSpecs = new PharmaciesSpecs(distinctPharmaciesIds);
            var pharmacies = await _unitOfWork.Repository<Pharmacy>().GetAllWithSpecAsync(pharmaciesSpecs);
            if (pharmacies is null)
                return BadRequest(new ApiResponse(404));

            // Find the nearest Pharmacies
            var result = _pharmacyService.GetNearestPharmacies(patientLocationWithMedicinesDto.Longtude, patientLocationWithMedicinesDto.Latitude, pharmacies) as List<PharmacyWithDistances>;
            return Ok();
        }
    }
}
