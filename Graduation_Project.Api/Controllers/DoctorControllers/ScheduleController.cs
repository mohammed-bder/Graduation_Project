using AutoMapper;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Specifications.WorkScheduleSpecs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class ScheduleController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IUnitOfWork unitOfWork, IMapper mapper, IScheduleService scheduleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPost("AddSchedule")]
        public async Task<ActionResult> AddSchedule(WorkScheduleFromUserDto workScheduleFromUser)
        {
            //Get current Doctor
            // 1️⃣ Validate Doctor Exists
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));

            // Check if the new schedule overlaps with an existing one
            bool isOverlapping = await _scheduleService
                .IsScheduleOverlappingAsync(doctorId, workScheduleFromUser.Day,
                                            workScheduleFromUser.StartTime, workScheduleFromUser.EndTime);

            if (isOverlapping)
                return BadRequest(new ApiResponse(400, "Schedule conflicts with an existing schedule."));

            var workSchedule = _mapper.Map<WorkScheduleFromUserDto, WorkSchedule>(workScheduleFromUser);
            workSchedule.DoctorId = doctorId;
            try
            {
                await _unitOfWork.Repository<WorkSchedule>().AddAsync(workSchedule);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add WorkSchedule"));
                }
                return Ok(workScheduleFromUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }


        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("GetAllDoctorSchedules")]
        public async Task<ActionResult<IReadOnlyList<WorkSchedule>>> GetAllDoctorSchedules()
        {
            // Validate Doctor Exists
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            // Get Work Schedules for the Doctor
            var wsSpec = new WorkSheduleSpecifications(doctorId);
            var schedules = await _unitOfWork.Repository<WorkSchedule>().GetAllWithSpecAsync(wsSpec);
            if (schedules.IsNullOrEmpty())
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<IReadOnlyList<WorkSchedule>, IReadOnlyList<WorkScheduleFromDatabaseDto>>(schedules));
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpDelete("DeleteSchedule/{id:int}")]
        public async Task<ActionResult> DeleteSchedule(int id)
        {
            // Check if the schedule exists
            var schedule = await _unitOfWork.Repository<WorkSchedule>().GetAsync(id);
            if (schedule == null)
                return NotFound(new ApiResponse(404, "Schedule not found"));

            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            if (schedule.DoctorId != doctorId)
                return Unauthorized(new ApiResponse(401, "This Schedule Doesnt belong to this Doctor"));

            try
            {
                // Delete the schedule
                _unitOfWork.Repository<WorkSchedule>().Delete(schedule);
                var result = await _unitOfWork.CompleteAsync();

                if (result <= 0)
                    return BadRequest(new ApiResponse(400, "Failed to delete schedule"));

                return Ok(new ApiResponse(200, "Schedule deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }



    }
}
