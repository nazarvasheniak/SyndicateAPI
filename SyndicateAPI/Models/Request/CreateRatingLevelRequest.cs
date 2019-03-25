using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class CreateRatingLevelRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int PointsCount { get; set; }
    }
}
