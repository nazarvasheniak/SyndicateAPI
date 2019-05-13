using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class UpdateRewardRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long IconID { get; set; }
    }
}
