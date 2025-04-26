using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class OrdersLast30DaysSpecification  : BaseSpecifications<PharmacyOrder>  
    {

        public OrdersLast30DaysSpecification(int pharmacyID) :
            base( po => po.PharmacyId == pharmacyID && po.OrderDate >= DateTime.Now.AddDays(-30))
        {
            
        }
    }
}
