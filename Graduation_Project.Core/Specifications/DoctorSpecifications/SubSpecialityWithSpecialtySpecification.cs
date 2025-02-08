using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class SubSpecialityWithSpecialtySpecification: BaseSpecifications<SubSpecialities>
    {
        public SubSpecialityWithSpecialtySpecification() : base()
        {
            Includes.Add(s => s.Specialty);
        }

        public SubSpecialityWithSpecialtySpecification(int id) : base(s => s.Id == id)
        {
            Includes.Add(s => s.Specialty);
        }
    }
}
