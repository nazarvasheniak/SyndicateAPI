using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SyndicateAPI.WebSocketManager.Interfaces
{
    public interface INotificationService
    {
        List<SocketUser> SocketUsers { get; }
        Task SendMessageAsync(WebSocket socket, string message);
        Task SendMessageAsync(string socketId, string message);
        Task SendMessageToAllAsync(string message);
    }
}
