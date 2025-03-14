using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithWorkScheduleSpecifications : BaseSpecifications<Doctor>
    {
        public DoctorWithWorkScheduleSpecifications(int id)
            : base(d =>
                d.Id == id
            )
        {
            Includes.Add(d => d.WorkSchedules);
            Includes.Add(d => d.ScheduleExceptions);
        }
    }
}
