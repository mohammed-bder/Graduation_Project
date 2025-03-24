using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.Shared
{
    public class PrescriptionImage : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }


        public int PrescriptionId { get; set; }

        [ForeignKey("PrescriptionId")]
        public Prescription Prescription { get; set; }
    }
}
