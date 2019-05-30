using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class PartnerProductViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public long PointsCount { get; set; }

        public PartnerProductViewModel() { }

        public PartnerProductViewModel(PartnerProduct product)
        {
            if (product != null)
            {
                ID = product.ID;
                Name = product.Name;
                Price = product.Price;
                PointsCount = product.PointsCount;
            }
        }
    }
}
