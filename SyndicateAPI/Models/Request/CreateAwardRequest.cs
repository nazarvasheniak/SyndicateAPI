using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class CreateAwardRequest
    {
        public long RewarderID { get; set; }
        public long RewardID { get; set; }
        public string Comment { get; set; }
    }
}
