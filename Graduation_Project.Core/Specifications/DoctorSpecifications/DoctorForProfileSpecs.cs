using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorForProfileSpecs : BaseSpecifications<Doctor>
    {
        public DoctorForProfileSpecs(string id):base(d => d.ApplicationUserId == id)
        {
            
        }

        
    }
}
