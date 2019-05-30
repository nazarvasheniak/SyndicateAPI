using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class GetOnlineStatusResponse : ResponseModel
    {
        public bool IsOnline { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
