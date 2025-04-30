using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class Top5MedicineSpecification : BaseSpecifications<MedicinePharmacyOrder>
    {
        public Top5MedicineSpecification() 
            : base()
        {
            Includes.Add(mpo => mpo.Medicine);
        }
    }
}
