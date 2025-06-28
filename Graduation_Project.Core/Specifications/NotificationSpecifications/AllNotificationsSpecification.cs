using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.NotificationSpecifications
{
    public class AllNotificationsSpecification : BaseSpecifications<NotificationRecipient>
    {
        public AllNotificationsSpecification(string userId) : base (n => n.UserId == userId)
        {
            Includes.Add(n => n.Notification);
        }
    }
}
