using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.Patients
{
    public class MedicalHistoryImage : BaseEntity
    {
        public string ImageUrl { get; set; }

        [ForeignKey("MedicalHistory")]
        public int MedicalHistoryId { get; set; }
        public MedicalHistory? MedicalHistory { get; set; } 
    }
}
