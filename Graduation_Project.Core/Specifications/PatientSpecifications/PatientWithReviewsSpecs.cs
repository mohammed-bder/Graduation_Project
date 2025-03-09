using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PatientSpecifications
{
    public class PatientWithReviewsSpecs : BaseSpecifications<Patient>
    {
        public PatientWithReviewsSpecs(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.Feedbacks);
        }
    }
}
