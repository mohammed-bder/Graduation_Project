using Microsoft.EntityFrameworkCore;
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
            ThenIncludes.Add(query => query.Include(p => p.MedicinePrescriptions)
                                       .ThenInclude(mp => mp.Medicine));
            Includes.Add(p => p.PrescriptionImages);
            Includes.Add(p => p.Doctor);
            Includes.Add(p => p.Patient);
        }

    }
}
