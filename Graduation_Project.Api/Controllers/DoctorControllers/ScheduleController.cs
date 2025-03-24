using AutoMapper;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Specifications.AppointmentSpecs;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.WorkScheduleSpecs;
using Graduation_Project.Service;
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
        private readonly IAppointmentService _appointmentService;

        public ScheduleController(IUnitOfWork unitOfWork, IMapper mapper, IScheduleService scheduleService, IAppointmentService appointmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduleService = scheduleService;
            _appointmentService = appointmentService;
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


        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPut("update-slot-duration/{durationMinutes:int}")]
        public async Task<IActionResult> UpdateSlotDuration(int durationMinutes)
        {
            // 1️⃣ Retrieve the doctor
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(new DoctorWithWorkScheduleSpecifications(doctorId));

            if (doctor == null)
                return NotFound(new ApiResponse(404, "Doctor not found."));

            if (durationMinutes <= 0)
                return BadRequest(new ApiResponse(400, "Slot duration must be greater than zero."));

            if (doctor.WorkSchedules.IsNullOrEmpty() && doctor.ScheduleExceptions.IsNullOrEmpty())
                return NotFound(new ApiResponse(404, "No WorkSchedules or Schedule Exceptions found for this Doctor."));

            // 2️⃣ Fetch all booked appointments for the doctor
            var appointmentSpec = new AppointmentSpecifications(doctor.Id, DateOnly.FromDateTime(DateTime.Today));
            var bookedAppointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(appointmentSpec);

            if (!bookedAppointments.Any())
            {
                // No appointments → Directly update slot duration
                doctor.SlotDurationMinutes = durationMinutes;
                _unitOfWork.Repository<Doctor>().Update(doctor);
                var result = await _unitOfWork.CompleteAsync();

                return result > 0 ? Ok(new ApiResponse(200, "Slot duration updated successfully."))
                                  : BadRequest(new ApiResponse(400, "Failed to change Slot Duration"));
            }

            // 3️⃣ Check if the new slot duration is smaller or bigger
            bool isReducingSlots = durationMinutes < doctor.SlotDurationMinutes;

            Dictionary<DateOnly, List<TimeOnly>> newSlotsDict = new();
            if (isReducingSlots)
            {
                // Dictionary to store newly generated slots per date

                DateOnly today = DateOnly.FromDateTime(DateTime.Today);
                int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

                // ✅ Traverse all days to regenerate slots for each date
                for (int i = 0; i < daysInMonth; i++)
                {
                    DateOnly currentDate = today.AddDays(i);
                    DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;

                    List<TimeOnly> newSlotsForDay = new();

                    // 🟢 1️⃣ Check for exceptions first
                    var exceptionsForDay = doctor.ScheduleExceptions.Where(e => e.Date == currentDate).ToList();
                    var schedulesForDay = doctor.WorkSchedules.Where(e => e.Day == currentDayOfWeek).ToList();
                    if (exceptionsForDay.Any())
                    {
                        foreach (var exception in exceptionsForDay)
                        {
                            if (!exception.IsAvailable)
                            {
                                newSlotsForDay.Clear(); // Block the day
                                break;
                            }

                            if (exception.StartTime.HasValue && exception.EndTime.HasValue)
                            {
                                newSlotsForDay.AddRange(_appointmentService.GenerateTimeSlots(new WorkSchedule
                                {
                                    StartTime = exception.StartTime.Value,
                                    EndTime = exception.EndTime.Value,
                                    Day = currentDayOfWeek
                                }, durationMinutes));
                            }
                        }
                    }
                    // 🔵 2️⃣ If no exception, use the default work schedule
                    else if (schedulesForDay.Any())
                    {
                        foreach (var schedule in schedulesForDay)
                        {
                            newSlotsForDay.AddRange(_appointmentService.GenerateTimeSlots(schedule, durationMinutes));
                        }
                    }

                    // ✅ Store the new slots
                    if (newSlotsForDay.Any())
                    {
                        newSlotsDict[currentDate] = newSlotsForDay;
                    }
                }
            }
            else
            {
                // 🔴 If the new slot duration is bigger, fewer slots will be available.
                foreach (var date in bookedAppointments.Select(a => a.AppointmentDate).Distinct())
                {
                    var workSchedule = doctor.WorkSchedules.FirstOrDefault(ws => ws.Day == date.DayOfWeek);
                    var existingAppointments = bookedAppointments
                        .Where(a => a.AppointmentDate == date)
                        .OrderBy(a => a.AppointmentTime)
                        .ToList();

                    int requiredSlots = existingAppointments.Count;

                    // 🟢 Generate slots for both normal & exception schedules
                    var newSlotList = new List<TimeOnly>();

                    if (workSchedule != null)
                    {
                        newSlotList.AddRange(_appointmentService.GenerateTimeSlots(workSchedule, durationMinutes));
                    }

                    foreach (var exception in doctor.ScheduleExceptions.Where(e => e.Date == date))
                    {
                        newSlotList.AddRange(_appointmentService.GenerateTimeSlots(new WorkSchedule
                        {
                            StartTime = exception.StartTime.Value,
                            EndTime = exception.EndTime.Value,
                            Day = date.DayOfWeek
                        }, durationMinutes));
                    }

                    int newSlotCapacity = newSlotList.Count;

                    // Check if the new slots will be enough
                    if (requiredSlots > newSlotCapacity)
                    {
                        int extraPatients = requiredSlots - newSlotCapacity;

                        // Try extending the normal schedule OR the exception
                        bool extended = await _scheduleService.ExtendWorkHoursIfPossible(doctor, date, extraPatients, durationMinutes);

                        if (!extended)
                        {
                            return BadRequest(new ApiResponse(400, $"Not enough slots available on {date}. Doctor must manually reschedule {extraPatients} appointments."));
                        }

                        // 🟢 Recalculate available slots after expansion
                        newSlotList.Clear();

                        if (workSchedule != null)
                        {
                            newSlotList.AddRange(_appointmentService.GenerateTimeSlots(workSchedule, durationMinutes));
                        }

                        foreach (var exception in doctor.ScheduleExceptions.Where(e => e.Date == date))
                        {
                            newSlotList.AddRange(_appointmentService.GenerateTimeSlots(new WorkSchedule
                            {
                                StartTime = exception.StartTime.Value,
                                EndTime = exception.EndTime.Value,
                                Day = date.DayOfWeek
                            }, durationMinutes));
                        }
                    }

                    // Update the dictionary with new slot lists
                    newSlotsDict[date] = newSlotList;
                }

            }

            // Adjust booked appointments to match new slots
            DateOnly? lastProcessedDate = null;
            List<TimeOnly>? availableSlots = null;

            foreach (var appointment in bookedAppointments.OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime))
            {
                // 🛠 Only fetch new slots when processing a new day
                if (lastProcessedDate != appointment.AppointmentDate)
                {
                    lastProcessedDate = appointment.AppointmentDate;
                    newSlotsDict.TryGetValue(appointment.AppointmentDate, out availableSlots);
                }

                if (availableSlots is not null && availableSlots.Count > 0)
                {
                    // Find the closest available slot to the original appointment time
                    var closestSlot = availableSlots.OrderBy(slot => Math.Abs(slot.Ticks - appointment.AppointmentTime.Ticks)).First();

                    // Assign the closest slot
                    appointment.AppointmentTime = closestSlot;
                    availableSlots.Remove(closestSlot); // Take the slot
                    _unitOfWork.Repository<Appointment>().Update(appointment);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, $"Not enough slots available on {appointment.AppointmentDate}. Doctor must reschedule."));
                }
            }

            // 📝 Save all updates
            // 4️⃣ Update slot duration in the doctor’s record
            doctor.SlotDurationMinutes = durationMinutes;
            _unitOfWork.Repository<Doctor>().Update(doctor);
            var saveResult = await _unitOfWork.CompleteAsync();

            return saveResult > 0 ? Ok(new ApiResponse(200, "Slot duration updated successfully."))
                                  : BadRequest(new ApiResponse(400, "Failed to change Slot Duration"));
        }


    }
}
