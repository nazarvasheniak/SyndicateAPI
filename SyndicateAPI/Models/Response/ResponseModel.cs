using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class ResponseModel
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "OK";
    }
}
