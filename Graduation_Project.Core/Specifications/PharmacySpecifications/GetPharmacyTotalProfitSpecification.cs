using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class GetPharmacyTotalProfitSpecification : BaseSpecifications<PharmacyOrder>
        
    {
        public GetPharmacyTotalProfitSpecification(int pharmacyID)
            : base(po => po.PharmacyId == pharmacyID)
        {
            
        }
    }
}
