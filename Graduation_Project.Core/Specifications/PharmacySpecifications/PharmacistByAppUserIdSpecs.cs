using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class PharmacistByAppUserIdSpecs : BaseSpecifications<Pharmacy>
    {
        public PharmacistByAppUserIdSpecs(string id) : base(p => p.ApplicationUserID == id)
        {
            
        }
    }
}
