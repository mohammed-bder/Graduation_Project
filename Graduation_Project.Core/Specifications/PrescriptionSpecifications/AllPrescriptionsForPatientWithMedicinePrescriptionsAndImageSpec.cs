using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PrescriptionSpecifications
{
    public class AllPrescriptionsForPatientWithMedicinePrescriptionsAndImageSpec: BaseSpecifications<Prescription>
    {
        public AllPrescriptionsForPatientWithMedicinePrescriptionsAndImageSpec(int patientId) : base(p => p.PatientId == patientId)
        {
            Includes.Add(p => p.MedicinePrescriptions);
            Includes.Add(p => p.PrescriptionImages);
        }
    }
}
