using SyndicateAPI.Domain.Models;

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
    }
}
