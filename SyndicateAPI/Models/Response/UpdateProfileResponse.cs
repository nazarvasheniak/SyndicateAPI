using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class UpdateProfileResponse : DataResponse<UserViewModel>
    {
        public long BonusPoints { get; set; } = 0;
    }
}
