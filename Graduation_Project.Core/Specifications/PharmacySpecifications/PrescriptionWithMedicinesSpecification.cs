using Graduation_Project.Core.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class PrescriptionWithMedicinesSpecification : BaseSpecifications<Prescription>
    {
        public PrescriptionWithMedicinesSpecification(int prescriptionId) : base(p => p.Id == prescriptionId)
        {
            ThenIncludes.Add(query => query.Include(p => p.MedicinePrescriptions)
                                       .ThenInclude(mp => mp.Medicine));
        }
    }
}
