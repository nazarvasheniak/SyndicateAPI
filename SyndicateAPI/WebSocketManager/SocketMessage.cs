using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.WebSocketManager
{
    public class SocketMessage
    {
        [JsonProperty("userID")]
        public long UserID { get; set; }
    }
}
