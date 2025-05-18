using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Core.Specifications.MedicineSpecifications
{
    public class OrderWithMedicinesQuantitySpecs : BaseSpecifications<PharmacyOrder>
    {
        public OrderWithMedicinesQuantitySpecs(int id) : base(o => o.Id == id)
        {
            Includes.Add(o => o.Patient);
            ThenIncludes.Add(o => o.Include(o => o.MedicinePharmacyOrders));
            ThenIncludes.Add(o => o.Include(o => o.Pharmacy).ThenInclude(o => o.pharmacyMedicineStocks));
        }
    }
}
