using Microsoft.AspNetCore.SignalR.Client;
using static System.Net.WebRequestMethods;

namespace Client.Services
{
    public class ChatService
    {
        private HubConnection _hubConnection;
        private readonly string _hubUrl;
        public event Action<string> OnMessageReceived;

        public ChatService(IConfiguration configuration)
        {
            _hubUrl = "http://localhost:5170/Chat";
        }

        public async Task InitializeConnection(string token)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveGroupMessage", (message) =>
            {
                OnMessageReceived?.Invoke(message);
            });

            await _hubConnection.StartAsync();
        }

        public async Task JoinGroup(string groupName)
        {
            await _hubConnection.InvokeAsync("JoinGroup", groupName);
        }

        public async Task SendMessage(string groupName, string message)
        {
            await _hubConnection.InvokeAsync("SendMessageToGroup", groupName, message);
        }
    }
}
