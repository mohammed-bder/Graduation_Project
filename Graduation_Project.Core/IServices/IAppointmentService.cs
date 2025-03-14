using Graduation_Project.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IAppointmentService
    {
        //public List<TimeOnly> GenerateTimeSlots(WorkSchedule schedule, int slotDurationMinutes);
        //public Task<Dictionary<DayOfWeek, List<TimeSpan>>> GetAvailableSlots(int doctorId, int slotDurationMinutes, DateTime selectedWeek);
        public Task<ServiceResult<Dictionary<DateOnly, List<TimeOnly>>>> GetAvailableSlotsAsync(Doctor doctor);
        public bool CheckIfPatientPayedVisita(Patient patient, bool payed);
    }
}
