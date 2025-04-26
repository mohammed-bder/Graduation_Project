using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.MedicineSpecifications
{
    public class PharamciesStockAvaliabilitySpecs : BaseSpecifications<PharmacyMedicineStock>
    {
        public PharamciesStockAvaliabilitySpecs(List<int> medicines) : base(p => medicines.Contains(p.MedicineId))
        {
            
        }
    }
}
