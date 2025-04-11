using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorSpecParams
    {
        private const int MaxPageSize = 10;  //default page size
        public int PageIndex { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { 
                search = value?.ToLower(); 
                ProcessSearch(); // Automatically process search when it's set
            }

        }

        public DateTime Today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Egypt Standard Time");
        public DateTime Tomorrow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Egypt Standard Time").AddDays(1);

        public AvailabilityFilter? Availability { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
        public string? Sort { get; set; }
        public int? SpecialtyId { get; set; }
        public int? SubSpecialtyId { get; set; }
        public int? RegionId { get; set; }
        public int? GovernorateId { get; set; }
        public Gender? Gender { get; set; }
        public string? FirstNameSearch { get; private set; }
        public string? LastNameSearch { get; private set; }

        // ProcessSearch Method
        private void ProcessSearch()
        {
            if (!string.IsNullOrEmpty(search))
            {
                var parts = search.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                FirstNameSearch = parts.Length > 0 ? parts[0] : null;
                LastNameSearch = parts.Length > 1 ? parts[1] : null;
            }
        }
    }


}
