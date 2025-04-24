using AutoMapper;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.DTO.Patients;
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

        public PharmacyController(IUnitOfWork unitOfWork, IMapper mapper ,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult FindNearestPharmacies(PatientLocationWithMedicinesDto patientLocationWithMedicinesDto)
        {
            return Ok();
        }

        [HttpGet("NearByPharmacies")]
        public async Task<IActionResult> GetNearByPharmacis([FromBody] LocationDTO locationDTO)
        {
            const double maxDistance = 10;

            var spec = new PharmacyWithDistanceSpecification();
            var pharmacies = await _unitOfWork.Repository<Pharmacy>().GetAllWithSpecAsync(spec);
            if(pharmacies is null || !pharmacies.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found."));

            var NearByPharmacies = pharmacies.Select(ph => new
            {
                phamacy = ph,
                distance = CalculateDistance(locationDTO.Latitude, locationDTO.Longitude, ph.Latitude, ph.Longitude)
            })
            .Where(d => d.distance <= maxDistance)
            .OrderBy(d => d.distance)
            .Select(d => new PharmacyCardDTO
            {
                Id = d.phamacy.Id,
                Name = d.phamacy.Name,
                PictureUrl = $"{_configuration["ServerUrl"]}/{ d.phamacy.ProfilePictureUrl }",
                Distance = (int)d.distance + " Km",
                Address = d.phamacy.Address,
                Location = $"https://www.google.com/maps?q={d.phamacy.Latitude},{d.phamacy.Longitude}",
                Contacts = d.phamacy.pharmacyContacts.Select(c => new PharmacyContactReturnDTO { PhoneNumber = c.PhoneNumber }).ToList()
            }).ToList();

            if(!NearByPharmacies.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No pharmacies found within the specified distance."));
                
            return Ok(NearByPharmacies);
        }

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
    }
}

