using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class OrdersSpecifications : BaseSpecifications<PharmacyOrder>
    {
        public OrdersSpecifications(int pharmacyId, int pageNumber, int pageSize, OrderStatus? orderStatus, DateTime? dateFilter)
                : base(O =>
                            O.PharmacyId == pharmacyId &&
                            (!orderStatus.HasValue || O.Status == orderStatus) &&
                            (!dateFilter.HasValue || O.OrderDate <= dateFilter.Value)
                )

        {
            Includes.Add(o => o.Patient);
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);
            
            // order By Date For Good User Experiance
            if (dateFilter != null)
                AddOrderBy(o => o.OrderDate);
            else
                AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersSpecifications(int pharmacyId, OrderStatus? orderStatus, DateTime? dateFilter)
                : base(O =>
                            O.PharmacyId == pharmacyId &&
                            (!orderStatus.HasValue || O.Status == orderStatus) &&
                            (!dateFilter.HasValue || O.OrderDate >= dateFilter.Value)
                )

        {
            
        }
    }
}
