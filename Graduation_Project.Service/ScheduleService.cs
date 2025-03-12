using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Specifications.ScheduleExceptionSpecs;

namespace Graduation_Project.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsScheduleOverlappingAsync(int doctorId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            return await _unitOfWork.Repository<WorkSchedule>()
                .AnyAsync(ws => ws.DoctorId == doctorId && ws.Day == day &&
                                ((startTime >= ws.StartTime && startTime < ws.EndTime) ||
                                 (endTime > ws.StartTime && endTime <= ws.EndTime) ||
                                 (startTime <= ws.StartTime && endTime >= ws.EndTime)));
        }

        public async Task<bool> IsScheduleOverlappingAsync(ScheduleException newException)
        {
            // Retrieve all existing exceptions for this doctor and date
            var existingExceptions = await _unitOfWork.Repository<ScheduleException>()
                .GetAllWithSpecAsync(new AllScheduleExceptionSpecifications(newException.DoctorId));

            if (!existingExceptions.Any())
                return false; // No existing exceptions → no overlap possible

            // Traverse through each existing exception and check for overlaps
            foreach (var existing in existingExceptions)
            {
                if (IsOverlapping(existing, newException))
                {
                    return true; // Found an overlap, return true
                }
            }

            return false; // No overlap found
        }

        public static bool IsOverlapping(ScheduleException exception1, ScheduleException exception2)
        {
            if (exception1.Date != exception2.Date) return false;
            if (!exception1.IsAvailable || !exception2.IsAvailable) return true;
            if (!exception1.StartTime.HasValue || !exception1.EndTime.HasValue ||
                !exception2.StartTime.HasValue || !exception2.EndTime.HasValue)
            {
                return false;
            }

            return exception1.StartTime.Value < exception2.EndTime.Value &&
                   exception1.EndTime.Value > exception2.StartTime.Value;
        }
    }
}
