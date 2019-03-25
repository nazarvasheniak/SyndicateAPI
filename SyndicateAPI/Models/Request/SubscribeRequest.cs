using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class SubscribeRequest
    {
        [Required]
        public long SubjectID { get; set; }
    }
}
