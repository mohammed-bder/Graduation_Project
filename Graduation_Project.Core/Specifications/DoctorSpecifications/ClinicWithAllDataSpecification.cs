using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class ClinicWithAllDataSpecification : BaseSpecifications<Clinic>
    {

        public ClinicWithAllDataSpecification() :base()
        {
            Includes.Add(c => c.Region);
            Includes.Add(C => C.ContactNumbers);
        }
    }
}
