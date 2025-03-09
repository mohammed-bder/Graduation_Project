using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.FeedBackSpecifications
{
    public class FeedBacksBetweenPatientAndDoctorSpecs : BaseSpecifications<Feedback>
    {
        public FeedBacksBetweenPatientAndDoctorSpecs(int doctorId,int patientId) : base(f => f.DoctorId == doctorId && f.PatientId == patientId) 
        {
            
        }
    }
}
