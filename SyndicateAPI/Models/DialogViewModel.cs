using SyndicateAPI.Domain.Models;
using System;

namespace SyndicateAPI.Models
{
    public class DialogViewModel
    {
        public long ID { get; set; }
        public UserViewModel Participant1 { get; set; }
        public UserViewModel Participant2 { get; set; }
        public DateTime StartDate { get; set; }
        public DialogMessageViewModel LastMessage { get; set; } 

        public DialogViewModel() { }

        public DialogViewModel(Dialog dialog, DialogMessage lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = new UserViewModel(dialog.Participant1);
                Participant2 = new UserViewModel(dialog.Participant2);
                StartDate = dialog.StartDate;
                LastMessage = new DialogMessageViewModel(lastMessage);
            }
        }

        public DialogViewModel(Dialog dialog, DialogMessageViewModel lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = new UserViewModel(dialog.Participant1);
                Participant2 = new UserViewModel(dialog.Participant2);
                StartDate = dialog.StartDate;
                LastMessage = lastMessage;
            }
        }

        public DialogViewModel(DialogViewModel dialog, DialogMessageViewModel lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = dialog.Participant1;
                Participant2 = dialog.Participant2;
                StartDate = dialog.StartDate;
                LastMessage = lastMessage;
            }
        }

        public DialogViewModel(DialogViewModel dialog, DialogMessage lastMessage)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = dialog.Participant1;
                Participant2 = dialog.Participant2;
                StartDate = dialog.StartDate;
                LastMessage = new DialogMessageViewModel(lastMessage);
            }
        }
    }
}
