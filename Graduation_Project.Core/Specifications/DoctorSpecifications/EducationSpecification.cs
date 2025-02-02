using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class EducationSpecification : BaseSpecifications<Education>
    {
        public EducationSpecification(int id) : base (e => e.Id == id)
        {
            
        }
        
    }
}
