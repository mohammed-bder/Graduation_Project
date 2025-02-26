using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.WorkScheduleSpecs
{
    public class WorkShedulewithDoctorAndDayCriteriaSpecifications : BaseSpecifications<WorkSchedule>
    {
        public WorkShedulewithDoctorAndDayCriteriaSpecifications(int doctorId, DayOfWeek day) : base(ws => ws.DoctorId == doctorId && ws.Day == day)
        {

        }
    }
}
