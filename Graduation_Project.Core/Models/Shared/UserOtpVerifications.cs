using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.Shared
{
    public class UserOtpVerifications : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public string OtpCode { get; set; }
        public OtpType OtpType { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsVerified { get; set; }
    }

}
