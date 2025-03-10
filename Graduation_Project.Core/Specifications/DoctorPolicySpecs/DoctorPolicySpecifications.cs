using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorPolicySpecs
{
    public class DoctorPolicySpecifications : BaseSpecifications<DoctorPolicy>
    {
        public DoctorPolicySpecifications(int doctorId)
            : base(dp =>
                dp.DoctorId == doctorId
            )
        {

        }
    }
}
