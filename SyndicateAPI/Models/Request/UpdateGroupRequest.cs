using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class UpdateGroupRequest
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string FullDesc { get; set; }
        public string Information { get; set; }
        public long AvatarID { get; set; }
    }
}
