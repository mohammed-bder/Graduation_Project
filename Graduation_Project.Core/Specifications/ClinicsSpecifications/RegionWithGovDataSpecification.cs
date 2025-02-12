using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.ClinicsSpecifications
{
    public class RegionWithGovDataSpecification : BaseSpecifications<Region>
    {

        public RegionWithGovDataSpecification(int id) :base(r => r.governorateId == id)
        {

            Includes.Add(r => r.governorate);
        }
    }
}
