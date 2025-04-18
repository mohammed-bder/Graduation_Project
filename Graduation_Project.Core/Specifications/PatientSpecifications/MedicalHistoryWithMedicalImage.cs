using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PatientSpecifications
{
    public class MedicalHistoryWithMedicalImage : BaseSpecifications<MedicalHistory>
    {
        public MedicalHistoryWithMedicalImage(int patientId , int medicalCategoryId) : base(m => m.PatientId == patientId && m.MedicalCategoryId == medicalCategoryId)
        {
            Includes.Add(m => m.medicalHistoryImage);
        }

    }
}
