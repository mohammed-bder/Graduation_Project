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

        [HttpGet]
        public async Task<ActionResult<Pharmacy>> FindNearestPharmacies(PatientLocationWithMedicinesDto patientLocationWithMedicinesDto)
        {
            // Find Pharmacies that have the medicines 
            // 1: Get Pharmacies Ids from PharmacyMedicineStock (M == M) Table
            var pharamciesStockAvaliabilitySpecs = new PharamciesStockAvaliabilitySpecs(patientLocationWithMedicinesDto.Medicines);
            var pharmacyMedicineStocks = await _unitOfWork.Repository<PharmacyMedicineStock>().GetAllWithSpecAsync(pharamciesStockAvaliabilitySpecs);
            if (pharmacyMedicineStocks is null)
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

        /********************************************* Get Near By Pharmacis by patient long ,lat  *********************************************/
        [HttpGet("NearByPharmacies")]
        public async Task<IActionResult> GetNearByPharmacis([FromBody] LocationDTO locationDTO)
        {
            const double maxDistance = 10;

            var spec = new PharmacyWithDistanceSpecification();
            var pharmacies = await _unitOfWork.Repository<Pharmacy>().GetAllWithSpecAsync(spec);

            if(pharmacies is null || !pharmacies.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found."));

            var NearByPharmacies = pharmacies.Select(ph => new PharmacyWithDistanceDTO
            {
                pharmacy = ph,
                distance = CalculateDistance(locationDTO.Latitude, locationDTO.Longitude, ph.Latitude, ph.Longitude)
            })
            .Where(d => d.distance <= maxDistance)
            .OrderBy(d => d.distance).ToList();
            
            var pharmacyCards = _mapper.Map<IReadOnlyList<PharmacyWithDistanceDTO> , IReadOnlyList<PharmacyCardDTO>>(NearByPharmacies);   

            if (!pharmacyCards.Any() || pharmacyCards is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found within the specified distance."));
                
            return Ok(pharmacyCards);
        }

        #region Calculate Distance
        /********************************************* Calculate Distance *********************************************/
        private double CalculateDistance(double lat1, double long1, double lat2, double long2)
        {
            const double R = 6371;
            var latDistance = DegreesToRadians(lat2 - lat1);
            var lonDistance = DegreesToRadians(long2 - long1);

            var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private  double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
        #endregion

    }
}

