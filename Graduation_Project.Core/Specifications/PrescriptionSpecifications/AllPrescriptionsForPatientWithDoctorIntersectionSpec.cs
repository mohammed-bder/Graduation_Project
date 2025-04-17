using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PrescriptionSpecifications
{
    public class AllPrescriptionsForPatientWithDoctorIntersectionSpec: BaseSpecifications<Prescription>
    {
        public AllPrescriptionsForPatientWithDoctorIntersectionSpec(int patientId, int doctorId) 
            : base(p =>
                p.PatientId == patientId && p.DoctorId == doctorId)
        {

        }
    }
}
