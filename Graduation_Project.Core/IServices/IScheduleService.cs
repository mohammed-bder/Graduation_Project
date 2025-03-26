using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IScheduleService
    {
        public Task<bool> ExtendWorkHoursIfPossible(Doctor doctor, DateOnly date, int extraPatients, int slotDuration);
        public Task<bool> IsScheduleOverlappingAsync(int doctorId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime);
        public Task<bool> IsScheduleOverlappingAsync(ScheduleException newException);
    }
}
