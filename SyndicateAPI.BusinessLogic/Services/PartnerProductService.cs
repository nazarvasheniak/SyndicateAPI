using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class PartnerProductService : BaseCrudService<PartnerProduct>, IPartnerProductService
    {
        public PartnerProductService(IRepository<PartnerProduct> repository) : base(repository)
        {
        }
    }
}
