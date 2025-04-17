using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PrescriptionSpecifications
{
    public class AllPrescriptionsForPatientSpec: BaseSpecifications<Prescription>
    {
        public AllPrescriptionsForPatientSpec(int patientId)
            : base(p =>
                p.PatientId == patientId)
        {
            Includes.Add(p => p.Doctor);
            ThenIncludes.Add(p => p.Include(p => p.Doctor.Specialty)); // ✅ Includes Specialty inside Doctor
        }
    }
}
