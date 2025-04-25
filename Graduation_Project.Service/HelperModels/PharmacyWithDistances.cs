using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.Models.Pharmacies;

namespace Graduation_Project.Service.HelperModels
{
    public class PharmacyWithDistances
    {
        public Pharmacy pharmacy { get; set; }
        public double Distance { get; set; }
    }
}
