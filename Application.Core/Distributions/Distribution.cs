using Application.Products;
using Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Distributions
{
    public enum DistributionWhen
    {
        Payed,
        Receipt,
        Complete
    }

    public enum BuyWhen
    {
        NoLimit,
        First,
        Next
    }

    public class Distribution:Entity
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public BuyWhen BuyWhen { get; set; }

        [Required]
        public int Level { get; set; }

        public decimal Money { get; set; }

        public float Ratio { get; set; }
    }
}
