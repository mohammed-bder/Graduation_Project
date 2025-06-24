using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class PharmacyWithItsContactsSpecification : BaseSpecifications<Pharmacy>
    {
        public PharmacyWithItsContactsSpecification() : base()
        {
            Includes.Add(p => p.pharmacyContacts);
        }

        public PharmacyWithItsContactsSpecification(int pharmacyId) : base(p => p.Id == pharmacyId)
        {
            Includes.Add(p => p.pharmacyContacts);
        }
    }
}
