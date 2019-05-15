using SyndicateAPI.Domain.Models;
using System;

namespace SyndicateAPI.Models
{
    public class UserViewModel
    {
        public long ID { get; set; }
        public string Nickname { get; set; }
        public long PointsCount { get; set; }
        public DateTime RegTime { get; set; }
        public FileViewModel Avatar { get; set; }
        public PersonViewModel Person { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }
        public bool IsOnline { get; set; }

        public UserViewModel() { }

        public UserViewModel(User user)
        {
            if (user != null)
            {
                ID = user.ID;
                Nickname = user.Nickname;
                PointsCount = user.PointsCount;
                RegTime = user.RegTime;
                Avatar = new FileViewModel(user.Avatar);
                Person = new PersonViewModel(user.Person);
                Coordinates = new CoordinatesViewModel(user.Latitude, user.Longitude);
                IsOnline = user.IsOnline;
            }
        }
    }
}
