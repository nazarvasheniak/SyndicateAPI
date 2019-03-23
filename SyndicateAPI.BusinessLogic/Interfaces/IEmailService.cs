using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendActivationMessage(string to, int code);
    }
}
