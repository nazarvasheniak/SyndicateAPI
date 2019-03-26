using SyndicateAPI.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SyndicateAPI.Models
{
    public class GroupViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string FullDesc { get; set; }
        public FileViewModel Avatar { get; set; }
        public UserViewModel Owner { get; set; }
        public List<GroupPostViewModel> Posts { get; set; }

        public GroupViewModel() { }

        public GroupViewModel(Group group)
        {
            if (group != null)
            {
                ID = group.ID;
                Name = group.Name;
                ShortDesc = group.ShortDesc;
                FullDesc = group.FullDesc;
                Avatar = new FileViewModel(group.Avatar);
                Owner = new UserViewModel(group.Owner);
            }
        }

        public GroupViewModel(Group group, IEnumerable<GroupPostViewModel> posts)
        {
            if (group != null)
            {
                ID = group.ID;
                Name = group.Name;
                ShortDesc = group.ShortDesc;
                FullDesc = group.FullDesc;
                Avatar = new FileViewModel(group.Avatar);
                Owner = new UserViewModel(group.Owner);
            }

            Posts = posts
                .Where(x => x.Post.IsPublished)
                .ToList();
        }

        public GroupViewModel(Group group, IEnumerable<GroupPost> posts)
        {
            if (group != null)
            {
                ID = group.ID;
                Name = group.Name;
                ShortDesc = group.ShortDesc;
                FullDesc = group.FullDesc;
                Avatar = new FileViewModel(group.Avatar);
                Owner = new UserViewModel(group.Owner);

                Posts = posts
                    .Where(x => x.Post.IsPublished)
                    .Select(x => new GroupPostViewModel(x))
                    .ToList();
            }
        }
    }
}
