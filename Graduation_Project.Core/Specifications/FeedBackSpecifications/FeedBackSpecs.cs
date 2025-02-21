using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.FeedBackSpecifications
{
    public class FeedBackSpecs : BaseSpecifications<Feedback>
    {
        public FeedBackSpecs(int id) : base (f => f.DoctorId == id)
        {
            Includes.Add(f => f.patient);
        }
    }
}
