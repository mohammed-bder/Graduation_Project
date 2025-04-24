using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.MedicineSpecifications
{
    public class PharmaciesSpecs : BaseSpecifications<Pharmacy>
    {
        public PharmaciesSpecs(List<int> Ids) : base(p => Ids.Contains(p.Id))
        {
            
        }
    }
}
