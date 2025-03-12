using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PrescriptionSpecifications
{
    public class PrescriptionWithMedicinePrescriptionsAndImageSpec: BaseSpecifications<Prescription>
    {
        public PrescriptionWithMedicinePrescriptionsAndImageSpec(int prescriptionId) : base(p => p.Id == prescriptionId)
        {
            Includes.Add(p => p.MedicinePrescriptions);
            Includes.Add(p => p.PrescriptionImages);
        }

    }
}
