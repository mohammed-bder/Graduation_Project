using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Specifications.MedicineSpecifications;
using Graduation_Project.Service.HelperModels;
using Graduation_Project.Api.DTO.Pharmacies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;

namespace Graduation_Project.Api.Controllers.PharmacyControllers
{
    public class PharmacyController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPharmacyService _pharmacyService;
        private readonly IMapper _mapper;

        public PharmacyController(IUnitOfWork unitOfWork, IPharmacyService pharmacyService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _pharmacyService = pharmacyService;
            _mapper = mapper;
        }

        [HttpGet("Find-Nearest-Pharmacy")]
        public async Task<ActionResult<List<PharmacyCardDTO>>> FindNearestPharmacies(PatientLocationWithMedicinesDto patientLocationWithMedicinesDto)
        {
            // Find Pharmacies That Contains the medicines 
            // 1: Get Pharmacies Ids from PharmacyMedicineStock (M == M) Table
            var pharamciesStockAvaliabilitySpecs = new PharamciesStockAvaliabilitySpecs(patientLocationWithMedicinesDto.Medicines);
            var pharmacyMedicineStocks = await _unitOfWork.Repository<PharmacyMedicineStock>().GetAllWithSpecAsync(pharamciesStockAvaliabilitySpecs);
            if(pharmacyMedicineStocks is null)
                return BadRequest(new ApiResponse(404));

            var requiredMedicineSet = patientLocationWithMedicinesDto.Medicines;
            var pharmacyIds = pharmacyMedicineStocks
                .GroupBy(s => s.PharmacyId)
                .Where(g =>
                {
                    var medicineSet = g
                        .Select(s => s.MedicineId)
                        .Distinct()
                        .ToHashSet();

                    return requiredMedicineSet.All(medicineSet.Contains);
                })
                .Select(g => g.Key)
                .ToList();

            // 2: Get Pharmacies
            var pharmaciesSpecs = new PharmaciesSpecs(pharmacyIds);
            var pharmacies = await _unitOfWork.Repository<Pharmacy>().GetAllWithSpecAsync(pharmaciesSpecs);
            if (pharmacies is null)
                return BadRequest(new ApiResponse(404));

            // Find the nearest Pharmacies
            var result = _pharmacyService.GetNearestPharmacies(patientLocationWithMedicinesDto.Longtude, patientLocationWithMedicinesDto.Latitude, pharmacies) as List<PharmacyWithDistances>;

            // Map to PharmacyCardDTO
            
            return Ok(_mapper.Map<List<PharmacyCardDTO>>(result));
        }

    }
}
