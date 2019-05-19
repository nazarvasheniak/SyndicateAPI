using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class SendDialogMessageRequest
    {
        [Required]
        public long RecipientID { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
