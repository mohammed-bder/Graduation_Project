using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorByAppUserIdSpecs : BaseSpecifications<Doctor>
    {
        public DoctorByAppUserIdSpecs(string id) : base(d => d.ApplicationUserId == id)
        {
            Includes.Add(d => d.Specialty);
        }
    }
}
