using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class PharmacyWithDistanceSpecification : BaseSpecifications<Pharmacy>
    {
        public PharmacyWithDistanceSpecification() : base()
        {
            Includes.Add(p => p.pharmacyContacts);
        }
    }
}
