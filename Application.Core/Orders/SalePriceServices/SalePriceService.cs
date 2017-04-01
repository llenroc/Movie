using Application.Orders.BoughtContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dependency;
using Infrastructure.Domain.Services;

namespace Application.Orders.SalePriceServices
{
    public class SalePriceService:DomainService
    {
        public BoughtContext Calculate(BoughtContext boughtContext)
        {
            foreach (BoughtItem boughtItem in boughtContext.BoughtItems)
            {
                boughtItem.Price = boughtItem.Specification.Price;
                boughtItem.Money = boughtItem.Price * boughtItem.Count;
                boughtContext.Money += boughtItem.Money;
            }
            return boughtContext;
        }
    }
}
