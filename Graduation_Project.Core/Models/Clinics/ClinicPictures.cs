using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.Clinics
{
    public class ClinicPictures : BaseEntity
    {


        [Required]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(Clinic))]
        public int ClinicId { get; set; }

        public Clinic Clinic { get; set; }
    }
}
