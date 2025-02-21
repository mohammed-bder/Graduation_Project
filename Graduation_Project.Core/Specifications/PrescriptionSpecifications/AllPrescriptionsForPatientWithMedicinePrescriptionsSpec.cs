using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PrescriptionSpecifications
{
    public class AllPrescriptionsForPatientWithMedicinePrescriptionsSpec: BaseSpecifications<Prescription>
    {
        public AllPrescriptionsForPatientWithMedicinePrescriptionsSpec(int patientId) : base(p => p.PatientId == patientId)
        {
            Includes.Add(p => p.MedicinePrescriptions);
        }
    }
}
