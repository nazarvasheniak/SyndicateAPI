using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class UserSessionService : BaseCrudService<UserSession>, IUserSessionService
    {
        // Session lifetime in minutes
        private readonly double SessionLifetime = 10;

        public UserSessionService(IRepository<UserSession> repository) : base(repository)
        {
        }

        public UserSession CreateSession(User user)
        {
            var session = new UserSession
            {
                User = user,
                StartDate = DateTime.UtcNow
            };

            Create(session);

            return session;
        }

        public bool IsSessionActive(User user)
        {
            var session = GetAll().FirstOrDefault(x => x.User == user);
            if (session == null)
                session = RefreshSession(user);

            if (session.StartDate.AddMinutes(SessionLifetime) < DateTime.UtcNow)
                return false;

            return true;
        }

        public UserSession RefreshSession(User user)
        {
            var session = GetAll().FirstOrDefault(x => x.User == user);

            if (session != null)
            {
                Delete(session);
                session = CreateSession(user);
            }
            else
            {
                session = CreateSession(user);
            }

            return session;
        }
    }
}
