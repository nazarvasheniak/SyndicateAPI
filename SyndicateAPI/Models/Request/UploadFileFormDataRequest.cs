using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class UploadFileFormDataRequest
    {
        public IFormFile File { get; set; }
    }
}
