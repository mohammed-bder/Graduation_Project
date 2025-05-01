using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class PharmacyMedicineLowStockSpecification : BaseSpecifications<PharmacyMedicineStock>
    {
        public PharmacyMedicineLowStockSpecification(int pharmacyID )
            : base(pmls => pmls.PharmacyId == pharmacyID && pmls.Quantity <= 5 )
        {
            Includes.Add(pmls => pmls.Medicine);
        }

    }
}
