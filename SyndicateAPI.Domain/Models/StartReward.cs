using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class StartReward : PersistentObject, IDeletableObject
    {
        public virtual User User { get; set; }
        public virtual bool IsAvatarCompleted { get; set; }
        public virtual bool IsBiographyCompleted { get; set; }
        public virtual bool IsVehicleCompleted { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
