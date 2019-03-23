using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class ActivationRequest
    {
        [Required]
        public long UserID { get; set; }

        [Required]
        public int Code { get; set; }
    }
}
