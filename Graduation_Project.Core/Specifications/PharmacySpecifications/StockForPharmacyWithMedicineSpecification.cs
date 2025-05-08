using Graduation_Project.Core.Models.Pharmacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class StockForPharmacyWithMedicineSpecification : BaseSpecifications<PharmacyMedicineStock>
    {
        public StockForPharmacyWithMedicineSpecification(int pharmacyId) 
            : base(
            pm => pm.PharmacyId == pharmacyId)
        {
            Includes.Add(m => m.Medicine);
        }
    }
}
