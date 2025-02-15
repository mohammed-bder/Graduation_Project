using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSubSpecialitySpecifications
{
    public class DoctorSubSpecialitySpecs : BaseSpecifications<DoctorSubspeciality>
    {
        public DoctorSubSpecialitySpecs(int doctorId,int subSpecialityId) 
                                        : base (d => d.DoctorId == doctorId && d.SubSpecialitiesId == subSpecialityId)
        {
            
        }
        public DoctorSubSpecialitySpecs(int doctorId)
                                        : base(d => d.DoctorId == doctorId)
        {

        }
    }
}
