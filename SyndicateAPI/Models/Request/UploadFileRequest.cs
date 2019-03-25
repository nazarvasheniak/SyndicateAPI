using SyndicateAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class UploadFileRequest
    {
        [Required]
        public byte[] Base64 { get; set; }

        [Required]
        public FileType Type { get; set; }
    }
}
