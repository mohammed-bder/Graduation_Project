using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Notifications;
using Graduation_Project.Service.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IFcmService _fcmService;
        private readonly UserManager<AppUser> _userManager;

        public NotificationService(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext, IFcmService fcmService, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _fcmService = fcmService;
            _userManager = userManager;
        }
        public async Task SendNotificationAsync(string userId, string message, string title)
        {
            // add notification 
            var notification = new Notification
            {
                Message = message,
                Title = title,
                CreatedDate = DateTime.Now,
            };
            await _unitOfWork.Repository<Notification>().AddWithSaveAsync(notification);

            // choose the recipient
            var notificationRecipients = new NotificationRecipient
            {
                IsRead = false,
                NotificationId = notification.Id,
                UserId = userId,
            };
            await _unitOfWork.Repository<NotificationRecipient>().AddWithSaveAsync(notificationRecipients);

            // Check if the user is online ==>(signalR + FCM)
            // or not active ==>(FCM)  
            if (NotificationHub.connectedUsers.ContainsKey(userId))
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message, title); // signalR

            var user = await _userManager.FindByIdAsync(userId);

            if (!string.IsNullOrEmpty(user?.DeviceToken))
                await _fcmService.SendFcmNotificationAsync(user.DeviceToken, message, title); // FCM

            await _unitOfWork.Repository<NotificationRecipient>().SaveAsync();
        }

        public async Task SendNotificationToGroupAsync(string groupName, string message, string title)
        {
            // add notification 
            var notification = new Notification
            {
                Message = message,
                Title = title,
                CreatedDate = DateTime.Now,
            };
            await _unitOfWork.Repository<Notification>().AddWithSaveAsync(notification);

            // send it to a group 
            var notificationRecipient = new NotificationRecipient
            {
                RecipientType = RecipientType.Patient,
                IsRead = false,
                NotificationId = notification.Id
            };
            await _unitOfWork.Repository<Notification>().AddWithSaveAsync(notification);

            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", message, title);

        }
    }
}
