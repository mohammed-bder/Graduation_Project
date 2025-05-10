using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class StockSpecParams
    {
        private const int MaxPageSize = 50;  //default page size
        public int PageIndex { get; set; } = 1;
        private int pageSize = 5;
        public int pharmacyId { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
