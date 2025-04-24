using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.Pharmacies
{
    public class PharmacyContact : BaseEntity
    {


        [Phone(ErrorMessage = "Please enter a valid contact number.")]
        public string PhoneNumber { get; set; }
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

    }
}
