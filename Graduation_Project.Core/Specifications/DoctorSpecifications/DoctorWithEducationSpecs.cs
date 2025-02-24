using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithEducationSpecs : BaseSpecifications<Doctor>
    {
        public DoctorWithEducationSpecs(int id) : base (d => d.Id == id)
        {
            Includes.Add(d => d.Education);
            Includes.Add(d => d.Specialty);
            Includes.Add(d => d.DoctorSubspeciality);

        }
    }
}
