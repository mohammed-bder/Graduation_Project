using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PatientSpecifications
{
    public class MedicalHistoryWithCategorySpec : BaseSpecifications<MedicalHistory>
    {
        public MedicalHistoryWithCategorySpec(int patientId) : base(mh => mh.PatientId == patientId)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(x => x.MedicalCategory);
        }

    }

}
