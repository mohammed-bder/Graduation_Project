using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.ClinicsSpecifications
{
    public class ClinicContactByNumber  : BaseSpecifications<ContactNumber>
    {
        public ClinicContactByNumber(string number) : base(cn => cn.PhoneNumber == number)
        {
            
        }
    }
}
