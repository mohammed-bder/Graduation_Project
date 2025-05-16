using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class StockWithMedicineSpecification : BaseSpecifications<PharmacyMedicineStock>
    {
        public StockWithMedicineSpecification(int Id)
            : base(
            pm => pm.Id == Id)
        {
            Includes.Add(m => m.Medicine);
        }
    }
}
