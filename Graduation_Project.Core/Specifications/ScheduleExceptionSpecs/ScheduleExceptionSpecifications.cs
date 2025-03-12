using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.ScheduleExceptionSpecs
{
    public class ScheduleExceptionSpecifications: BaseSpecifications<ScheduleException>
    {
        public ScheduleExceptionSpecifications(int id)
            : base(e => e.Id == id)
        {

        }
    }
}
