using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.SendingEmail
{
    public class Email : BaseEntity
    {
        public string Subject { get; set; }
        public string Recipients { get; set; }
        public string Body { get; set; }

    }
}
