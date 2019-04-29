using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class GroupCreatorService : BaseCrudService<GroupCreator>, IGroupCreatorService
    {
        public GroupCreatorService(IRepository<GroupCreator> repository) : base(repository)
        {
        }
    }
}
