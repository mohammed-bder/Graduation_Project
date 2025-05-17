using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.Models.Pharmacies;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class OrderWithMedicinesSpecs : BaseSpecifications<PharmacyOrder>
    {
        public OrderWithMedicinesSpecs(int id) : base(o => o.Id == id)
        {
            Includes.Add(o => o.Patient);
            ThenIncludes.Add(o => o.Include(o => o.MedicinePharmacyOrders).ThenInclude(o => o.Medicine));
        }
    }
}
