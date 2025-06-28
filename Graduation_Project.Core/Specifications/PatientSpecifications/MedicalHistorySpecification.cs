using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PatientSpecifications
{
    public class MedicalHistorySpecification : BaseSpecifications<MedicalHistory>
    {
        public MedicalHistorySpecification(int id) : base(m => m.Id == id)
        {
            Includes.Add(m => m.Patient);
        }
    }
}
