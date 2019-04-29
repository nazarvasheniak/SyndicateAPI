using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class GetListRequest
    {
        [Required]
        public int PageCount { get; set; }

        [Required]
        public int PageNumber { get; set; }
    }
}
