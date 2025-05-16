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
using Graduation_Project.Api.Attributes;
using Graduation_Project.Api.DTO.Shared;
using Graduation_Project.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Repository.Identity;
using System.Collections.ObjectModel;

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

        /********************************************* Search on Medicine By Name  *********************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetMedicineInfoByName")]
        public async Task<ActionResult<IReadOnlyList<SearchMedicinesResponseDTO>>> GetMedicinesInfoByName([FromQuery] string? name, [FromQuery] int count = 20)// count will increase with every showMore
        {
            // check on name
            if (string.IsNullOrEmpty(name))
                return BadRequest(new ApiResponse(400, "Please enter a medicine name"));

            // Get Matched medicines
            var spec = new MedicineSpec(name, count);
            var matchedMedicines =
                        await _unitOfWork.Repository<Medicine>().GetAllWithSpecAsync(spec, m => new SearchMedicinesResponseDTO { Id = m.Id, Name = m.Name_en , Price = $"{m.Price} EGP"  , DosageForm = m.DosageForm });

            if (matchedMedicines.IsNullOrEmpty())
                return NotFound(new ApiResponse(404, "No medicines found with this name"));

            // mapping
            return Ok(matchedMedicines);
        }

        /********************************************* Find Nearest Pharmacies which have specific medicines  *********************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPost("Find-Nearest-Pharmacies")]
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
            var result = _pharmacyService.GetNearestPharmacies((double)patientLocationWithMedicinesDto.Longtude,(double) patientLocationWithMedicinesDto.Latitude, pharmacies) as List<PharmacyWithDistances>;

            var output1 = _mapper.Map<List<PharmacyCardDTO>>(result);
            // Map to PharmacyCardDTO
            return Ok(output1);
        }

        /********************************************* Get Near By Pharmacis by patient long ,lat  *********************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("NearByPharmacies")]
        public async Task<ActionResult<List<PharmacyCardDTO>>> GetNearByPharmacis([FromQuery] LocationDTO locationDTO)
        {
            const double maxDistance = 10;
            // 1- Include pharmacy contact with pharmacy 
            var spec = new PharmacyWithItsContactsSpecification();
            var pharmacies = await _unitOfWork.Repository<Pharmacy>().GetAllWithSpecAsync(spec);

            if(pharmacies is null || !pharmacies.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found."));

            // 2- Calculate distance between the pharmacy and the patient location then order them 
            var NearByPharmacies = _pharmacyService.GetDefaultNearestPharmacies((double)locationDTO.Longitude,(double)locationDTO.Latitude,pharmacies) as List<PharmacyWithDistances>;
            if (NearByPharmacies is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No Nearest pharmacies found."));

            var pharmacyCards = _mapper.Map<IReadOnlyList<PharmacyWithDistances> , IReadOnlyList<PharmacyCardDTO>>(NearByPharmacies);   

            if (!pharmacyCards.Any() || pharmacyCards is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found within the specified distance."));
                
            return Ok(pharmacyCards);
        }

        /********************************************* Get Medicine From Prescription by Id  *********************************************/

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetMedicineFromPrescription/{prescriptionId:int}")]
        public async Task<ActionResult<List<MedicineDTO>>> GetMedicineFromPrescription(int prescriptionId)
        {
            // get prescription info includeing medicinePrescription then include medicine
            var prescriptionSpecs = new PrescriptionWithMedicinesSpecification(prescriptionId);
            var prescription = await _unitOfWork.Repository<Prescription>().GetWithSpecsAsync(prescriptionSpecs);
            if (prescription is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No prescription found."));

            // map to medicineDTO
            var medicines = _mapper.Map<List<MedicineDTO>>(prescription.MedicinePrescriptions);

            if (medicines is null || !medicines.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No medicines found."));

            return Ok(medicines);
        }

        /********************************************* Add New Order *********************************************/

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPost("Add-Order")]
        public async Task<ActionResult> AddOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto.MedicinesDictionary == null || !orderDto.MedicinesDictionary.Any())
            {
                return BadRequest("At least one medicine must be provided.");
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = new PharmacyOrder()
                {
                    PharmacyId = orderDto.PharmacyId,
                    PatientId = int.Parse(User.FindFirstValue(Identifiers.PatientId)),
                    Status = OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    //MedicinePharmacyOrders = medicinePharmacyOrder
                };

                var newOrder = await _unitOfWork.Repository<PharmacyOrder>().AddWithSaveAsync(order);

                var medicinePharmacyOrders = new Collection<MedicinePharmacyOrder>();
                foreach (var item in orderDto.MedicinesDictionary)
                {
                    medicinePharmacyOrders.Add(new MedicinePharmacyOrder()
                    {
                        MedicineId = item.Key,
                        PharmacyOrderId = newOrder.Id,
                        Quantity = item.Value,
                    });
                }

                await _unitOfWork.Repository<MedicinePharmacyOrder>().AddRangeAsync(medicinePharmacyOrders);
                await _unitOfWork.Repository<MedicinePharmacyOrder>().SaveAsync();

                await transaction.CommitAsync();

                return Ok(new ApiResponse(StatusCodes.Status201Created, "Created Successfully"));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while creating the order.");
            }

        }
    }
}

