using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PrescriptionSpecifications
{
    public class PrescriptionWithMedicinePrescriptionsSpec: BaseSpecifications<Prescription>
    {
        public PrescriptionWithMedicinePrescriptionsSpec(int prescriptionId) : base(p => p.Id == prescriptionId)
        {
            Includes.Add(p => p.MedicinePrescriptions);
        }

    }
}
