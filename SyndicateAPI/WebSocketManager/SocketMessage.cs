using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.WebSocketManager
{
    public class SocketMessage
    {
        [JsonProperty("type")]
        public SocketMessageType Type { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }
    }
}
