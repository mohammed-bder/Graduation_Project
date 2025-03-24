using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.ClinicsSpecifications
{
    public class ClinicByDoctorIdSpecification : BaseSpecifications<Clinic>
    {
        public ClinicByDoctorIdSpecification(int doctorId) : base(c => c.DoctorId == doctorId)
        {
            
        }
    }
}
