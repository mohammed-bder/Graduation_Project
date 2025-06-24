using AutoMapper;
using Graduation_Project.Api.DTO.Orders;
using Graduation_Project.Api.DTO.Pharmacies;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
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
                return BadRequest(new ApiResponse(500, "An error occurred while creating the order."));
            }

        }
    }
}
