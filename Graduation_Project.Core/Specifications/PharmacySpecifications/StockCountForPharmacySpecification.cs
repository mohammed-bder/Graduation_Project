using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class StockCountForPharmacySpecification : BaseSpecifications<PharmacyMedicineStock>
    {
        public StockCountForPharmacySpecification(StockSpecParams specParams)
            : base(pm =>
                (pm.PharmacyId == specParams.pharmacyId)
                &&
                (
                    string.IsNullOrEmpty(specParams.Search) ||
                    pm.Medicine.Name_en.ToLower().StartsWith(specParams.Search)
                )
            )
        {

        }
    }
}
