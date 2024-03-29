﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class PublishPostRequest
    {
        [Required]
        public long ImageID { get; set; }

        public string Text { get; set; }
        public DateTime PublishTime { get; set; }
        public ulong RatingScore { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
    }
}
