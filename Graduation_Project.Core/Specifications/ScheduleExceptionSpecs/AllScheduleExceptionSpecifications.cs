using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.ScheduleExceptionSpecs
{
    public class AllScheduleExceptionSpecifications: BaseSpecifications<ScheduleException>
    {
        public AllScheduleExceptionSpecifications(int doctorId)
        : base(e => e.DoctorId == doctorId)
        {

        }
    }
}
