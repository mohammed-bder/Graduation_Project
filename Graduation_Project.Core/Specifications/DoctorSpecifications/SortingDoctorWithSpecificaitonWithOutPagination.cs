using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class SortingDoctorWithSpecificaitonWithOutPagination : SortingDoctorWithSpecificaiton
    {
        public SortingDoctorWithSpecificaitonWithOutPagination(DoctorSpecParams specParams) : base (specParams)
        {
            IsPaginationEnabled = false;
        }
    }
}
