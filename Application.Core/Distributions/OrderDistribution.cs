using Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Distributions
{
    public class OrderDistribution : Entity
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public decimal OrderMoney { get; set; }

        [Required]
        public decimal Money { get; set; }

        [Required]
        public int OrderItemId{get;set;}

        [Required]
        public long UserId { get; set; }

        [Required]
        public int DistributionId { get; set; }
    }
}
