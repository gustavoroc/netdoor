using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Infrastructure
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            var userName = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId} has joined the group");
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            var userName = Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId}: {message}");
        }
    }
}
