using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.DTOs
{
    public class SlotDto
    {
        public TimeOnly Time { get; set; }
        public bool IsAvailable { get; set; }
    }
}
