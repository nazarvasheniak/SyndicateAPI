using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Interfaces
{
    public interface IDeletableObject
    {
        /// <summary>
        /// Архивирован
        /// </summary>
        bool Deleted { get; set; }
    }
}
