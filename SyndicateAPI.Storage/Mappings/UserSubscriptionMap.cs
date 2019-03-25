using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class UserSubscriptionMap : ClassMap<UserSubscription>
    {
        public UserSubscriptionMap()
        {
            Table("user_subscriptions");

            Id(u => u.ID, "id");

            References(e => e.Subject, "id_subject");
            References(e => e.Subscriber, "id_subscriber");

            Map(u => u.IsActive, "is_active");
            Map(u => u.Deleted, "deleted");
        }
    }
}
