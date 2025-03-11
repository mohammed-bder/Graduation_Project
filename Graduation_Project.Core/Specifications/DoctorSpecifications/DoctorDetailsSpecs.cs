using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorDetailsSpecs : BaseSpecifications<Doctor>
    {
        public DoctorDetailsSpecs(int id) : base(d => d.Id == id)
        {
            Includes.Add(d => d.Appointments);
            Includes.Add(d => d.Specialty);

        }
    }
}
