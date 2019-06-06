using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IStartRewardService : IBaseCrudService<StartReward>
    {
        StartReward CreateStartReward(User user);
    }
}
