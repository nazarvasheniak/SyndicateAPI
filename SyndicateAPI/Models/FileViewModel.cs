﻿using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Models
{
    public class FileViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }
        public string Url { get; set; }

        public FileViewModel() { }

        public FileViewModel(File file)
        {
            if (file != null)
            {
                ID = file.ID;
                Name = file.Name;
                Type = file.Type;
                Url = file.Url;
            }
        }
    }
}
