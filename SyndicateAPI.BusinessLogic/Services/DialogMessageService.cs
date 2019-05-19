using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class DialogMessageService : BaseCrudService<DialogMessage>, IDialogMessageService
    {
        public DialogMessageService(IRepository<DialogMessage> repository) : base(repository)
        {
        }
    }
}
