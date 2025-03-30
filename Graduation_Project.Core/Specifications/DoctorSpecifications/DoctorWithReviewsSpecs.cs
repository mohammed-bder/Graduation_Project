using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithReviewsSpecs : BaseSpecifications<Doctor>
    {
        public DoctorWithReviewsSpecs(int id) : base (d => d.Id == id)
        {
            Includes.Add(d => d.Feedbacks);
        }
    }
}
