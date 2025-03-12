using AutoMapper;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Repository;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Core.Specifications.ScheduleExceptionSpecs;
using Graduation_Project.Api.DTO.Shared;
using Graduation_Project.Service;

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class ScheduleExceptionController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduleService _scheduleService;

        public ScheduleExceptionController(IUnitOfWork unitOfWork, IMapper mapper, IScheduleService scheduleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduleService = scheduleService;
        }

        [HttpPost("AddScheduleException")]
        public async Task<ActionResult> AddScheduleException([FromBody] ScheduleExceptionFromUserDto scheduleExceptionFromUser)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(scheduleExceptionFromUser.DoctorId);
            if (doctor == null)
                return NotFound(new ApiResponse(404, "Doctor not found"));

            var exception = _mapper.Map<ScheduleExceptionFromUserDto, ScheduleException>(scheduleExceptionFromUser);
            // Check if it overlaps with existing exceptions
            bool isOverlapping = await _scheduleService.IsScheduleOverlappingAsync(exception);
            if (isOverlapping)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Schedule exception overlaps with an existing one."));
            }
            try
            {
                await _unitOfWork.Repository<ScheduleException>().AddAsync(exception);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add Schedule Exception"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            return Ok(scheduleExceptionFromUser);
        }

        [HttpGet("GetAllScheduleException/{doctorId:int}")]
        public async Task<ActionResult> GetScheduleExceptions(int doctorId)
        {
            var exceptionsFromDB = await _unitOfWork.Repository<ScheduleException>()
                .GetAllWithSpecAsync(new AllScheduleExceptionSpecifications(doctorId));

            if (exceptionsFromDB.IsNullOrEmpty())
            {
                return NotFound(new ApiResponse(404, "No schedule exceptions found for this doctor."));
            }

            return Ok(_mapper.Map<IReadOnlyList<ScheduleException>, IReadOnlyList<ScheduleExceptionFromDatabaseDto>>(exceptionsFromDB));
        }

        [HttpDelete("DeleteException/{id:int}")]
        public async Task<ActionResult> DeleteScheduleException(int id)
        {
            var exception = await _unitOfWork.Repository<ScheduleException>()
                .GetWithSpecsAsync(new ScheduleExceptionSpecifications(id));

            if (exception == null)
            {
                return NotFound(new ApiResponse(404, "Schedule exception not found."));
            }

            _unitOfWork.Repository<ScheduleException>().Delete(exception);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to Delete Schedule Exception"));
            }
            return Ok(new ApiResponse(200, "Schedule Exception Deleted Successfully"));
        }
    }
}
