using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.SecretarySpecifications
{
    public class SecretaryByAppuserIdSpecification : BaseSpecifications<Secretary>
    {
        public SecretaryByAppuserIdSpecification(string id): base(s => s.ApplicationUserId == id)
        {
            Includes.Add(s => s.doctors);
        }
    }
}
