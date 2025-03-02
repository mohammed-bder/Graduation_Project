using Graduation_Project.Api.DTO.Shared;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Specifications.AppointmentSpecs;
using Graduation_Project.Core.Specifications.WorkScheduleSpecs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.Shared
{
    public class AppointmentController : BaseApiController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentController(IAppointmentService appointmentService, IUnitOfWork unitOfWork)
        {
            _appointmentService = appointmentService;
            _unitOfWork = unitOfWork;
        }

        //[HttpGet("available-slots/{doctorId:int}")]
        //public async Task<IActionResult> GetAvailableSlots(int doctorId, int slotDurationMinutes, DateOnly selectedWeek)
        //{
        //    try
        //    {
        //        var spec = new WorkSheduleSpecifications(doctorId);
        //        var schedules = await _unitOfWork.Repository<WorkSchedule>().GetAllWithSpecAsync(spec);
        //        if (schedules.IsNullOrEmpty())
        //        {
        //            return NotFound(new ApiResponse(404));
        //        }

        //        var appointmentSpec = new AppointmentSpecifications(doctorId, selectedWeek);
        //        var bookedAppointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(appointmentSpec);

        //        Dictionary<DayOfWeek, List<TimeOnly>> availableSlots = new();

        //        foreach (var schedule in schedules)
        //        {
        //            var slots = _appointmentService.GenerateTimeSlots(schedule, slotDurationMinutes);

        //            // Remove booked slots for this day
        //            var bookedTimesForDay = bookedAppointments
        //                .Where(a => a.AppointmentDate.DayOfWeek == schedule.Day)
        //                .Select(a => a.AppointmentTime)
        //                .ToList();

        //            slots = slots.Where(slot => !bookedTimesForDay.Contains(slot)).ToList();

        //            if (slots.Count > 0)
        //            {
        //                if (!availableSlots.ContainsKey(schedule.Day))
        //                    availableSlots[schedule.Day] = new List<TimeOnly>();

        //                availableSlots[schedule.Day].AddRange(slots);
        //            }
        //        }
        //        return Ok(availableSlots);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse(400, ex.Message));
        //    }

        //}

        [HttpGet("available-slots/{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<IActionResult> GetAvailableSlots(int id, int slotDurationMinutes, DateOnly selectedWeekStart)
        {
            try
            {
                var spec = new WorkSheduleSpecifications(id);
                var schedules = await _unitOfWork.Repository<WorkSchedule>().GetAllWithSpecAsync(spec);
                if (schedules.IsNullOrEmpty())
                {
                    return NotFound(new ApiResponse(404, "No work schedules found for this doctor."));
                }

                var appointmentSpec = new AppointmentSpecifications(id, selectedWeekStart);
                var bookedAppointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(appointmentSpec);

                Dictionary<DayOfWeek, List<TimeOnly>> availableSlots = new();

                foreach (var schedule in schedules)
                {
                    var slots = _appointmentService.GenerateTimeSlots(schedule, slotDurationMinutes);

                    // Filter out booked slots
                    var bookedTimesForDay = bookedAppointments
                        .Where(a => a.AppointmentDate.DayOfWeek == schedule.Day)
                        .Select(a => a.AppointmentTime)
                        .ToList();

                    slots = slots.Except(bookedTimesForDay).ToList();

                    if (slots.Any())
                    {
                        availableSlots.TryAdd(schedule.Day, new List<TimeOnly>());
                        availableSlots[schedule.Day].AddRange(slots);
                    }
                }

                return Ok(availableSlots);
            }

            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }


        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto request)
        {
            // 1️⃣ Validate Doctor Exists
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(request.doctorId);
            if (doctor == null)
                return NotFound(new ApiResponse(404)); // Doctor not found

            // 2️⃣ Validate Patient Exists
            var patient = await _unitOfWork.Repository<Patient>().GetAsync(request.patientId);
            if (patient == null)
                return NotFound(new ApiResponse(404)); ; // Patient not found

            // 3️⃣ Check if the doctor is available on the given day
            var dayOfWeek = request.date.DayOfWeek;

            var wsSpec = new WorkShedulewithDoctorAndDayCriteriaSpecifications(request.doctorId, dayOfWeek);
            var workSchedules = await _unitOfWork.Repository<WorkSchedule>().GetAllWithSpecAsync(wsSpec);

            if (!workSchedules.Any())
                return NotFound(new ApiResponse(404, "Doctor doesn't work that day")); // Doctor doesn't work that day

            // 4️⃣ Check if the selected time falls within any shift
            bool isTimeValid = workSchedules.Any(ws => request.time >= ws.StartTime && request.time < ws.EndTime);
            if (!isTimeValid)
                return NotFound(new ApiResponse(404, "Time is outside of work schedule")); // Time is outside of work schedule

            // 5️⃣ Check if the slot is already booked
            var aSpec = new AppointmentWithDoctorAndDateAndTimeCriteriaSpecifications(request.doctorId, request.date, request.time);
            var isAlreadyBooked = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(aSpec);

            if (isAlreadyBooked.Any())
                return NotFound(new ApiResponse(404, "Slot already taken")); // Slot already taken

            // 6️⃣ Create and Save Appointment
            var appointment = new Appointment
            {
                DoctorId = request.doctorId,
                PatientId = request.patientId,
                AppointmentDate = request.date,
                AppointmentTime = request.time
            };

            await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
            var result = await _unitOfWork.CompleteAsync();
            if (result == 0)
            {
                return BadRequest(new ApiResponse(400));
            }

            return Ok(new ApiResponse(200, "Appointment booked successfully!"));
        }
    }
}
