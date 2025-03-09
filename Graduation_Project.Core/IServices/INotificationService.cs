using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message, string title);
        Task SendNotificationToGroupAsync(string groupName, string message, string title);

    }
}
