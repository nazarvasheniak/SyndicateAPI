using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class AdminAuthorizationResponse : ResponseModel
    {
        public string Token { get; set; }
    }
}
