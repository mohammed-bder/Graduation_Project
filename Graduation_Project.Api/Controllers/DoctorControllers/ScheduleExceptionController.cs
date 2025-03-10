using AutoMapper;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Repository;
using Graduation_Project.Api.DTO.Doctors;

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class ScheduleExceptionController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleExceptionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("AddScheduleException")]
        public async Task<IActionResult> AddScheduleException([FromBody] ScheduleExceptionFromUserDto scheduleExceptionFromUser)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(scheduleExceptionFromUser.DoctorId);
            if (doctor == null)
                return NotFound(new ApiResponse(404, "Doctor not found"));

            var exception = new ScheduleException
            {
                DoctorId = doctor.Id,
                Date = scheduleExceptionFromUser.Date,
                StartTime = scheduleExceptionFromUser.StartTime,
                EndTime = scheduleExceptionFromUser.EndTime,
                IsAvailable = scheduleExceptionFromUser.IsAvailable
            };

            await _unitOfWork.Repository<ScheduleException>().AddAsync(exception);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add Schedule Exception"));
            }

            return CreatedAtAction(nameof(GetScheduleExceptions), new { doctorId = scheduleExceptionFromUser.DoctorId }, exception);
        }

        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetScheduleExceptions(int doctorId)
        {
            //var exceptions = await _unitOfWork.Repository<ScheduleException>()
              //  .GetAllWithSpecAsync(new ScheduleExceptionSpecifications(doctorId));

            //if (!exceptions.Any())
            //{
            //    return NotFound(new ApiResponse(404, "No schedule exceptions found for this doctor."));
            //}

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleException(int id)
        {
            //var exception = await _unitOfWork.Repository<ScheduleException>().GetByIdAsync(id);

            //if (exception == null)
            //{
            //    return NotFound(new ApiResponse(404, "Schedule exception not found."));
            //}

            //_unitOfWork.Repository<ScheduleException>().Delete(exception);
            //await _unitOfWork.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}
