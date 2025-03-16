using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graduation_Project.Core.DTOs
{
    public class PatientDTO : UserDto
    {
        public int? Points { get; set; }
        public BloodType? BloodType { get; set; }
        public int? Age { get; set; }
    }
}
