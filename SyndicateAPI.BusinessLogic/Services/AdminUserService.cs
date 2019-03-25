using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class AdminUserService : BaseCrudService<AdminUser>, IAdminUserService
    {
        public AdminUserService(IRepository<AdminUser> repository) : base(repository)
        {
        }
    }
}
