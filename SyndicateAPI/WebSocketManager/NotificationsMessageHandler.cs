﻿using Newtonsoft.Json;
using SyndicateAPI.WebSocketManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SyndicateAPI.WebSocketManager
{
    public class NotificationsMessageHandler : WebSocketHandler, INotificationService
    {
        public List<SocketUser> socketUsers = new List<SocketUser>();

        public List<SocketUser> SocketUsers
        {
            get => socketUsers;
        }

        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            //var myTimer = new System.Timers.Timer();
            //myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            //myTimer.Interval = 3000;
            //myTimer.Start();
        }

        public async Task SendUpdateToUser(long userID, SocketMessageType type, object message)
        {
            var socketUser = socketUsers.FirstOrDefault(x => x.UserID == userID);
            if (socketUser == null)
                return;

            var msg = JsonConvert.SerializeObject(new SocketMessageUpdate(type, message));

            await SendMessageAsync(socketUser.ID, msg);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var socketUser = socketUsers.FirstOrDefault(x => x.ID.Equals(socketId));

            if (socketUser != null)
                socketUsers.Remove(socketUser);

            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var jsonMessage = JsonConvert.DeserializeObject<SocketMessage>(message);

            try
            {
                var existSocketUser = socketUsers.FirstOrDefault(x => x.ID.Equals(socketId));
                if (existSocketUser == null)
                    socketUsers.Add(new SocketUser { ID = socketId, UserID = jsonMessage.UserID });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
