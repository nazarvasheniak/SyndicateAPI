using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class AdminUserViewModel
    {
        public long ID { get; set; }
        public string Login { get; set; }
        public PersonViewModel Person { get; set; }

        public AdminUserViewModel() { }

        public AdminUserViewModel(AdminUser adminUser)
        {
            if (adminUser != null)
            {
                ID = adminUser.ID;
                Login = adminUser.Login;
                Person = new PersonViewModel(adminUser.Person);
            }
        }
    }
}
