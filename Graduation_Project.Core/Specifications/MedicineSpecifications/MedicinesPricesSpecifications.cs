using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.MedicineSpecifications
{
    public class MedicinesPricesSpecifications : BaseSpecifications<Medicine>
    {
        public MedicinesPricesSpecifications(List<int> ids) : base (m => ids.Contains(m.Id))
        {
            ApplySelector(m => m.Price);
        }
    }
}
