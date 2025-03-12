using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PatientSpecifications
{
    public class PatientByAppUserIdSpecs : BaseSpecifications<Patient>
    {
        public PatientByAppUserIdSpecs(string id) : base(p => p.ApplicationUserId == id)
        {

        }
    }
}
