using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class CreateGroupRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortDesc { get; set; }

        [Required]
        public string FullDesc { get; set; }

        [Required]
        public long AvatarID { get; set; }
    }
}
