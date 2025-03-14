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
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Core.Constants;
using System.Security.Claims;

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

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPost("AddScheduleException")]
        public async Task<ActionResult> AddScheduleException([FromBody] ScheduleExceptionFromUserDto scheduleExceptionFromUser)
        {
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));

            var exception = _mapper.Map<ScheduleExceptionFromUserDto, ScheduleException>(scheduleExceptionFromUser);
            exception.DoctorId = doctorId;
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

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("GetAllScheduleException")]
        public async Task<ActionResult> GetScheduleExceptions()
        {
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            var exceptionsFromDB = await _unitOfWork.Repository<ScheduleException>()
                .GetAllWithSpecAsync(new AllScheduleExceptionSpecifications(doctorId));

            if (exceptionsFromDB.IsNullOrEmpty())
            {
                return NotFound(new ApiResponse(404, "No schedule Exceptions found for this doctor."));
            }

            return Ok(_mapper.Map<IReadOnlyList<ScheduleException>, IReadOnlyList<ScheduleExceptionFromDatabaseDto>>(exceptionsFromDB));
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpDelete("DeleteException/{id:int}")]
        public async Task<ActionResult> DeleteScheduleException(int id)
        {
            var exception = await _unitOfWork.Repository<ScheduleException>().GetAsync(id);
            if (exception == null)
                return NotFound(new ApiResponse(404, "Schedule Exception not found"));

            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            if (exception.DoctorId != doctorId)
                return Unauthorized(new ApiResponse(401, "This Schedule Exception Doesnt belong to this Doctor"));

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
