using Microsoft.AspNetCore.SignalR;

namespace ChatService.Infrastructure
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId} has joined the group");
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId}: {message}");
        }
    }
}
