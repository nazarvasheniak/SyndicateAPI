using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class UpdatePostRequest
    {
        public string Text { get; set; }
        public DateTime PublishTime { get; set; }
        public ulong RatingScore { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
    }
}
