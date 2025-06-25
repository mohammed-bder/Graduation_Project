using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.AppointmentSpecs
{
    public class AppointmentSpecParams
    {
        public AppointmentSpecParams(string? s, int id)
        {
            search = s ?? string.Empty;
            Id = id;
        }
        private string? search;
        public int Id { get; set; }
        public string? Search
        {
            get { return search; }
            set
            {
                search = value?.ToLower();
                ProcessSearch(); // Automatically process search when it's set
            }

        }
        public string? FirstNameSearch { get; private set; }
        public string? LastNameSearch { get; private set; }

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
