using Graduation_Project.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.DTOs;

namespace Graduation_Project.Core.IServices
{
    public interface IAppointmentService
    {
        public List<TimeOnly> GenerateTimeSlots(WorkSchedule schedule, int slotDurationMinutes);
        public List<SlotDto> GenerateTimeeSlots(WorkSchedule schedule, int slotDurationMinutes);
        //public Task<ServiceResult<Dictionary<DateOnly, List<TimeOnly>>>> GetAvailableSlotsAsync(Doctor doctor);
        public Task<ServiceResult<Dictionary<DateOnly, List<SlotDto>>>> GetAvailableSlotsAsync(Doctor doctor);
        public bool CheckIfPatientPayedVisita(Patient patient, bool payed);
    }
}
