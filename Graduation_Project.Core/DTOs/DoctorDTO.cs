using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graduation_Project.Core.DTOs
{
    public class DoctorDTO : UserDto
    {
        public string Speciality { get; set; }
        public string? Description { get; set; }
    }
}
