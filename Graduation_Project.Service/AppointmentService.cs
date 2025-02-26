using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;

namespace Graduation_Project.Service
{
    //public class AppointmentService : IAppointmentService
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public AppointmentService(IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public List<TimeOnly> GenerateTimeSlots(WorkSchedule schedule, int slotDurationMinutes)
    //    {
    //        var slots = new List<TimeOnly>();
    //        TimeOnly currentSlot = schedule.StartTime;

    //        while (schedule.StartTime < schedule.EndTime)
    //        {
    //            slots.Add(schedule.StartTime);
    //            schedule.StartTime = schedule.StartTime.AddMinutes(slotDurationMinutes);
    //        }

    //        return slots;
    //    }
    //}
}
