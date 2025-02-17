using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithFeedbackSpecs :BaseSpecifications<Feedback>
    {
        public DoctorWithFeedbackSpecs(int id) : base(f => f.DoctorId == id)
        {
            ApplySelector(f => f.Score);
        }
    }
}
