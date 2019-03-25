using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class PublishPostRequest
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime PublishTime { get; set; }

        [Required]
        public long ImageID { get; set; }

        [Required]
        public long RatingLevelID { get; set; }
    }
}
