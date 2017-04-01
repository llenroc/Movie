using Application.Distributions;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Tenants.Dto
{
    [AutoMap(typeof(Distribution))]
    public class DistributionDto:EntityDto
    {
        public int ProductId { get; set; }

        public BuyWhen BuyWhen { get; set; }

        public int Level { get; set; }

        public decimal Money { get; set; }

        public float Ratio { get; set; }
    }
}
