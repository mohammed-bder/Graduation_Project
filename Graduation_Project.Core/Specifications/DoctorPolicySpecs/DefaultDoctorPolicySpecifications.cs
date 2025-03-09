using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorPolicySpecs
{
    public class DefaultDoctorPolicySpecifications : BaseSpecifications<DoctorPolicy>
    {
        public DefaultDoctorPolicySpecifications()
            : base(dp =>
                 dp.IsDefault
            )
        {

        }
    }
}
