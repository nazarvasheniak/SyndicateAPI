using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class PointsCodeViewModel
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public long PointsCount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiresDate { get; set; }
        public bool IsUsed { get; set; }
        public UserViewModel Creator { get; set; }

        public PointsCodeViewModel() { }

        public PointsCodeViewModel(PointsCode code)
        {
            if (code != null)
            {
                ID = code.ID;
                Code = code.Code;
                PointsCount = code.PointsCount;
                CreationDate = code.CreationDate;
                ExpiresDate = code.ExpiresDate;
                IsUsed = code.IsUsed;
                Creator = new UserViewModel(code.Creator);
            }
        }
    }
}
