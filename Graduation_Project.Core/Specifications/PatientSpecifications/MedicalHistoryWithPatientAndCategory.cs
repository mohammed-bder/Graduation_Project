using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PatientSpecifications
{
    public class MedicalHistoryWithPatientAndCategory : BaseSpecifications<MedicalHistory> 
    {
        public MedicalHistoryWithPatientAndCategory() : base()
        {
            AddIncludes();
        }

        public MedicalHistoryWithPatientAndCategory(int id) : base(h => h.Id == id)
        {

        }
        private void AddIncludes()
        {
            Includes.Add(x => x.Patient);
            Includes.Add(x => x.MedicalCategory);
        }
    }
}
