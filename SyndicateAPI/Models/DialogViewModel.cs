using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class DialogViewModel
    {
        public long ID { get; set; }
        public UserViewModel Participant1 { get; set; }
        public UserViewModel Participant2 { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsUnread { get; set; }
        public List<DialogMessageViewModel> Messages { get; set; } 

        public DialogViewModel() { }

        public DialogViewModel(Dialog dialog, IEnumerable<DialogMessage> messages)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = new UserViewModel(dialog.Participant1);
                Participant2 = new UserViewModel(dialog.Participant2);
                StartDate = dialog.StartDate;
                Messages = messages
                    .Select(x => new DialogMessageViewModel(x))
                    .OrderByDescending(x => x.Time)
                    .ToList();

                if (Messages.Count == 0)
                {
                    IsUnread = false;
                }
                else
                {
                    if (!Messages.FirstOrDefault().IsReaded)
                        IsUnread = true;
                    else
                        IsUnread = false;
                }
            }
        }

        public DialogViewModel(Dialog dialog, IEnumerable<DialogMessageViewModel> messages)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = new UserViewModel(dialog.Participant1);
                Participant2 = new UserViewModel(dialog.Participant2);
                StartDate = dialog.StartDate;
                Messages = messages
                    .OrderByDescending(x => x.Time)
                    .ToList();

                if (Messages.Count == 0)
                {
                    IsUnread = false;
                }
                else
                {
                    if (!Messages.FirstOrDefault().IsReaded)
                        IsUnread = true;
                    else
                        IsUnread = false;
                }
            }
        }

        public DialogViewModel(DialogViewModel dialog, IEnumerable<DialogMessageViewModel> messages)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = dialog.Participant1;
                Participant2 = dialog.Participant2;
                StartDate = dialog.StartDate;
                Messages = messages
                    .OrderByDescending(x => x.Time)
                    .ToList();

                if (Messages.Count == 0)
                {
                    IsUnread = false;
                }
                else
                {
                    if (!Messages.FirstOrDefault().IsReaded)
                        IsUnread = true;
                    else
                        IsUnread = false;
                }
            }
        }

        public DialogViewModel(DialogViewModel dialog, IEnumerable<DialogMessage> messages)
        {
            if (dialog != null)
            {
                ID = dialog.ID;
                Participant1 = dialog.Participant1;
                Participant2 = dialog.Participant2;
                StartDate = dialog.StartDate;
                Messages = messages
                    .Select(x => new DialogMessageViewModel(x))
                    .OrderByDescending(x => x.Time)
                    .ToList();

                if (Messages.Count == 0)
                {
                    IsUnread = false;
                }
                else
                {
                    if (!Messages.FirstOrDefault().IsReaded)
                        IsUnread = true;
                    else
                        IsUnread = false;
                }
            }
        }
    }
}
