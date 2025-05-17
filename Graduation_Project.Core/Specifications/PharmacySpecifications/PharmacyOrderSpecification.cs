using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class PharmacyOrderSpecification : BaseSpecifications<PharmacyOrder>
    {

        public PharmacyOrderSpecification(int pharmacyID , bool isOnlyPending = false) 
            : base( po => po.PharmacyId == pharmacyID && (!isOnlyPending || po.Status == OrderStatus.Pending))
        {
            Includes.Add(po => po.Patient);
        }


        //public PharmacyOrderSpecification(int pharmacyID, bool isToday = false)
        // : base(po => po.PharmacyId == pharmacyID && (!isToday || po.OrderDate.Date == DateTime.Now.Date))
        //{
        //    Includes.Add(po => po.Patient);
        //}
    }
}
