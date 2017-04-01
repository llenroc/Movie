using Application.CustomerInfos.Dto;
using Application.Orders.BoughtContexts;
using Infrastructure.AutoMapper;
using System.Collections.Generic;

namespace Application.Orders.Fronts.Products.Dto
{
    [AutoMapFrom(typeof(ProductBoughtContext))]
    public class OrderConfirmOutput
    {
        public List<BoughtItemOutput> BoughtItems { get; set; }

        public bool IsNeedLogistics { get; set; }

        public CustomerInfoDto CustomerInfo { get; set; }

        public decimal Money { get; set; }
    }
}
