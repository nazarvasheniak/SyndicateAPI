using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class ListResponse<T> : ResponseModel
    {
        public List<T> Data { get; set; }
        public Pagination Pagination { get; set; }
    }
}
