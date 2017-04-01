using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain.Entities.Auditing;
using Infrastructure.Domain.Entities;
using Application.Regions;

namespace Application.Shops
{
    public enum ReduceStockType
    {
        WhenCreateOrder,
        WhenPay
    }

    public class Shop:FullAuditedEntity,IMustHaveTenant
    {
        public int TenantId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public ReduceStockType ReduceStockType { get; set; }
        public virtual Address Address { get; set; }

        public string GetShopUrl()
        {
            return "";
        }
    }
}
