using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class DialogService : BaseCrudService<Dialog>, IDialogService
    {
        public DialogService(IRepository<Dialog> repository) : base(repository)
        {
        }
    }
}
