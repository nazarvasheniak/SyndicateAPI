using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class DataResponse<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}
