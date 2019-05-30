using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.WebSocketManager
{
    public class SocketMessageUpdate
    {
        [JsonProperty("type")]
        public SocketMessageType Type { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        public SocketMessageUpdate(SocketMessageType type, object message)
        {
            Type = type;
            Message = message;
        }
    }
}
