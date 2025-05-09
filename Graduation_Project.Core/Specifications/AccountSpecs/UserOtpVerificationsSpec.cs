using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.AccountSpecs
{
    public class UserOtpVerificationsSpec : BaseSpecifications<UserOtpVerifications>
    {
        public UserOtpVerificationsSpec(string userId, OtpType otpType)
            : base(o => o.ApplicationUserId == userId && o.OtpType == otpType)
        {
            AddOrderByDescending(o => o.ExpiresOn);
        }
    }
}
