﻿using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class StartRewardService : BaseCrudService<StartReward>, IStartRewardService
    {
        public StartRewardService(IRepository<StartReward> repository) : base(repository)
        {
        }

        public StartReward CreateStartReward(User user)
        {
            var startReward = new StartReward
            {
                IsAvatarCompleted = false,
                IsBiographyCompleted = false,
                IsVehicleCompleted = false,
                User = user
            };

            Create(startReward);

            return startReward;
        }
    }
}
