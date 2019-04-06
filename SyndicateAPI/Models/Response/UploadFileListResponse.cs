using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class UploadFileListResponse : ResponseModel
    {
        public List<long> FileIDList { get; set; }
    }
}
