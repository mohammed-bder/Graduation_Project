using AutoMapper;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Specifications.MedicineSpecifications;
using Graduation_Project.Service.HelperModels;
using Graduation_Project.Api.DTO.Pharmacies;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Helpers;
using Graduation_Project.Core;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Runtime.CompilerServices;
using AutoMapper;
using System.Collections.Generic;
using Graduation_Project.Core.Constants;

namespace Graduation_Project.Api.Controllers.PharmacyControllers
{
    public class PharmacyController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IPharmacyService _pharmacyService;

        public PharmacyController(IUnitOfWork unitOfWork, IMapper mapper ,IConfiguration configuration,IPharmacyService pharmacyService )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _pharmacyService = pharmacyService;
        }

        [HttpGet("Find-Nearest-Pharmacies")]
        public async Task<ActionResult<List<PharmacyCardDTO>>> FindNearestPharmacies([FromBody] PatientLocationWithMedicinesDto patientLocationWithMedicinesDto)
        {
            // Find Pharmacies That Contains the medicines 
            // 1: Get Pharmacies Ids from PharmacyMedicineStock (M == M) Table
            var pharamciesStockAvaliabilitySpecs = new PharamciesStockAvaliabilitySpecs(patientLocationWithMedicinesDto.Medicines);
            var pharmacyMedicineStocks = await _unitOfWork.Repository<PharmacyMedicineStock>().GetAllWithSpecAsync(pharamciesStockAvaliabilitySpecs);
            if(pharmacyMedicineStocks is null)
                return BadRequest(new ApiResponse(404,"Medicines Not Avaliables"));

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

            var output1 = _mapper.Map<List<PharmacyCardDTO>>(result);
            // Map to PharmacyCardDTO
            return Ok(output1);
        }

        /********************************************* Get Near By Pharmacis by patient long ,lat  *********************************************/
        [HttpGet("NearByPharmacies")]
        public async Task<ActionResult<List<PharmacyCardDTO>>> GetNearByPharmacis([FromBody] LocationDTO locationDTO)
        {
            const double maxDistance = 10;
            // 1- Include pharmacy contact with pharmacy 
            var spec = new PharmacyWithDistanceSpecification();
            var pharmacies = await _unitOfWork.Repository<Pharmacy>().GetAllWithSpecAsync(spec);

            if(pharmacies is null || !pharmacies.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found."));

            // 2- Calculate distance between the pharmacy and the patient location then order them 
            var NearByPharmacies = _pharmacyService.GetDefaultNearestPharmacies(locationDTO.Longitude,locationDTO.Latitude,pharmacies) as List<PharmacyWithDistances>;
            if (NearByPharmacies is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No Nearest pharmacies found."));

            var pharmacyCards = _mapper.Map<IReadOnlyList<PharmacyWithDistances> , IReadOnlyList<PharmacyCardDTO>>(NearByPharmacies);   

            if (!pharmacyCards.Any() || pharmacyCards is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found within the specified distance."));
                
            return Ok(pharmacyCards);
        }

    }
}

