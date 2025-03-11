using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.IServices;

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
    }
}
