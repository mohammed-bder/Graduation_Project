using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Service.Hubs
{
    public sealed class NotificationHub : Hub
    {
        public static ConcurrentDictionary<string, List<string>> connectedUsers = new(); // A thread-safe dictionary that tracks all connected users.
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                connectedUsers.AddOrUpdate(
                     userId,
                     new List<string> { Context.ConnectionId },
                     // if the user is active in another machine
                     (userId, existingUsers) =>
                     {
                         existingUsers.Add(userId);
                         return existingUsers;
                     });
            }
            
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // check if the connected user is exist or not 
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var connectedUserId = Context.ConnectionId;
            if (!string.IsNullOrEmpty(userId))
            {
                // check if current user (userId) is exist in the ConcurrentDictionary
                if (connectedUsers.ContainsKey(userId))
                {
                    connectedUsers[userId].Remove(connectedUserId);

                    // check if the current user have an emtpy list value in the ConcurrentDictionary and remove it 
                    if (!connectedUsers[userId].Any())
                        connectedUsers.TryRemove(userId, out _); // Remove (Key, Value)
                }
                
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
