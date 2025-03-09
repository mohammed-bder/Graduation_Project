using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Service.Hubs
{
    public sealed class NotificationHub : Hub
    {
        //public async Task SendNotification(string message)
        //{
        //    await Clients.All.SendAsync("ReceiveNotification", message);
        //}

        //public async Task SendNotificationToUser(string userId, string title, string message)
        //{
        //    await Clients.User(userId).SendAsync("ReceiveNotification", message, title);
        //}

        //public override Task OnConnectedAsync()
        //{
        //    Console.WriteLine("Hello i am solved");
        //    return base.OnConnectedAsync();
        //}
    }
}
