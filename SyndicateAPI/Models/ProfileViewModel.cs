﻿using SyndicateAPI.Domain.Models;
using System.Collections.Generic;

namespace SyndicateAPI.Models
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string GroupName { get; set; }
        public GroupViewModel Group { get; set; }
        public long PointsCount { get; set; }
        public string RatingLevel { get; set; }
        public string CityName { get; set; }
        public string AvatarUrl { get; set; }
        public long SubscribersCount { get; set; }
        public string Biography { get; set; }
        public List<AwardViewModel> Awards { get; set; }
        public List<VehicleViewModel> Vehicles { get; set; }
        public List<UserViewModel> Subscribers { get; set; }
        public List<UserViewModel> Subscriptions { get; set; }
        public bool IsOnline { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }
    }
}
