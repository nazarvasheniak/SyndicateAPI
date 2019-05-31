using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class SubmitPointsCodeResponse : ResponseModel
    {
        public long PointsAwarded { get; set; }
    }
}
