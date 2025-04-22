using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Graduation_Project.Core.IServices;

namespace Graduation_Project.Service
{
    public class FcmNotificationService : IFcmService
    {
        public FcmNotificationService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "FireBaseMessaging", "balto-95152-firebase-adminsdk-fbsvc-0f7a82df3a.json");

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credentialsPath)
                });
            }
        }
        public async Task SendFcmNotificationAsync(string deviceToken, string message, string title)
        {
            var firebaseMessage = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = message
                }
            };

            var result = await FirebaseMessaging.DefaultInstance.SendAsync(firebaseMessage);
            Console.WriteLine($"FCM Sent: {result}");
        }
    }
}
