using SyndicateAPI.Domain.Models;
using System;

namespace SyndicateAPI.Models
{
    public class DialogViewModel
    {
        public long ID { get; set; }
        public UserViewModel FromUser { get; set; }
        public UserViewModel ToUser { get; set; }
        public DateTime StartDate { get; set; }
        public DialogMessageViewModel LastMessage { get; set; } 

        public DialogViewModel() { }

        public DialogViewModel(Dialog dialog, DialogMessage lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                FromUser = new UserViewModel(dialog.FromUser);
                ToUser = new UserViewModel(dialog.ToUser);
                StartDate = dialog.StartDate;
                LastMessage = new DialogMessageViewModel(lastMessage);
            }
        }

        public DialogViewModel(Dialog dialog, DialogMessageViewModel lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                FromUser = new UserViewModel(dialog.FromUser);
                ToUser = new UserViewModel(dialog.ToUser);
                StartDate = dialog.StartDate;
                LastMessage = lastMessage;
            }
        }

        public DialogViewModel(DialogViewModel dialog, DialogMessageViewModel lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                FromUser = dialog.FromUser;
                ToUser = dialog.ToUser;
                StartDate = dialog.StartDate;
                LastMessage = lastMessage;
            }
        }

        public DialogViewModel(DialogViewModel dialog, DialogMessage lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                FromUser = dialog.FromUser;
                ToUser = dialog.ToUser;
                StartDate = dialog.StartDate;
                LastMessage = new DialogMessageViewModel(lastMessage);
            }
        }
    }
}
