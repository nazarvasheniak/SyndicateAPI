using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class CreatePointsCodeRequest
    {
        [Required]
        public long PointsCount { get; set; }
    }
}
