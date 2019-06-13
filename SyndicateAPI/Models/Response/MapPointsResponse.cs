using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class MapPointsResponse : ResponseModel
    {
        public List<MapPointViewModel> Points { get; set; }
        public List<PartnerViewModel> Partners { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> GroupUsers { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public List<GroupPostViewModel> GroupPosts { get; set; }
    }
}
