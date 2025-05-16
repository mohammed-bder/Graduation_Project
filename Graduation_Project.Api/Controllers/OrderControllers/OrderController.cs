using AutoMapper;
using Graduation_Project.Api.DTO.Orders;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Api.Controllers.OrderControllers
{
    public class OrderController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /************************** Get Order View (Pharmacy info && Patient Info) **************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("OrderView/{pharmacyId:int}")]
        public async Task<ActionResult<OrderViewDTO>> GetOrderView(int pharmacyId)
        {
            // Get Patient id 
            var patientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));

            // Get Patient info
            var patient = await _unitOfWork.Repository<Patient>().GetAsync(patientId);  
            if(patient == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound , "Patient Not Found"));
            }

            // Get Pharmacy info
            var spec = new PharmacyWithItsContactsSpecification(pharmacyId);
            var pharmacy = await _unitOfWork.Repository<Pharmacy>().GetWithSpecsAsync(spec);
            if(pharmacy == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound , "Pharmacy Not Found"));
            }

            // Map the data to OrderViewDTO 
            var orderViewResponse = _mapper.Map<OrderViewDTO>(pharmacy);
            orderViewResponse.PatientPhoneNumber = patient.PhoneNumber;
            orderViewResponse.PatientAddress = patient.Address;

            return Ok(orderViewResponse);
        }
    }
}
