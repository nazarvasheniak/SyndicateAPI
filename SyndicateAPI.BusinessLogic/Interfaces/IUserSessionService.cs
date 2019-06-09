using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IUserSessionService : IBaseCrudService<UserSession>
    {
        UserSession CreateSession(User user);
        UserSession RefreshSession(User user);
        bool IsSessionActive(User user);
    }
}
