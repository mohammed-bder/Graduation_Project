using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithSpecialitySpecs : BaseSpecifications<Doctor>
    {
        public DoctorWithSpecialitySpecs(string userId) : base (d => d.ApplicationUserId == userId) 
        {
            Includes.Add(d => d.Specialty);
        }
    }
}
