using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class Pagination
    {
        public int PageNumber { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public Pagination(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
