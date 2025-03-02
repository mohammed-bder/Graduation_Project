using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.MedicineSpecifications
{
    public class MedicineSpec : BaseSpecifications<Medicine>
    {
        public MedicineSpec(string? name,int take) : base(m => m.Name.ToLower().StartsWith(name))
        {
            Selector = m => new
            {
                m.Id,
                m.Name
            };

            ApplyPagination(0, take);
        }
    }
}
