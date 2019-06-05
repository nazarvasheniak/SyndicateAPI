using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class SetOnlineStatusRequest
    {
        [Required]
        public bool IsOnline { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
