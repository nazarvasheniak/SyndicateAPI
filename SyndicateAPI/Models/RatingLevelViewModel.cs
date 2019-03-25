using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class RatingLevelViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public int PointsCount { get; set; }

        public RatingLevelViewModel() { }

        public RatingLevelViewModel(RatingLevel ratingLevel)
        {
            if (ratingLevel != null)
            {
                ID = ratingLevel.ID;
                Title = ratingLevel.Title;
                PointsCount = ratingLevel.PointsCount;
            }
        }
    }
}
