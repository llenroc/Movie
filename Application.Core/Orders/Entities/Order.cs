using Application.Authorization.Users;
using Application.Entities;
using Application.Expresses;
using Application.Wallets.Entities;
using Infrastructure;
using Infrastructure.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Entities
{
    public enum OrderStatus
    {
        Declined,
        UnPay,
        UnShip,
        Shiped,
        Received,
        Commented,
        Completed,
        Refunded,
        Close,
    }

    public enum RefundStatus
    {
        ToRefund,
        Refunding,
        Refunded,
        Failed
    }

    public enum PaymentStatus
    {
        ToPay,
        Payed
    }

    public enum ShipStatus
    {
        UnShip,
        Shipping,
        Received
    }

    public class Order : FullAuditedEntity, IUserIdentifierEntity
    {
        public int TenantId { get; set; }

        [Required]
        [MaxLength(32)]
        public string Number { get; set; }

        public string ShopId { get; set; }

        [Required]
        public string Title { get; set; }

        public OrderStatus OrderStatus { get;set;}

        [Required]
        public decimal Money { get; set; }

        [Required]
        public decimal PayMoney { get; set; }

        [Required]
        public long UserId { get; set; }
        
        public virtual User User { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? PaymentDatetime { get; set; }

        public PayType PayType { get; set; }

        public RefundStatus RefundStatus { get; set; }

        [Required]
        public bool IsNeedShip { get; set; }

        public bool IsNeedLogistics { get; set; }

        public ShipStatus ShipStatus { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual OrderCustomerInfo OrderCustomerInfo { get; set; }

        public int? ExpressCompanyId { get; set; }

        public string TrackingNumber { get; set; }

        public virtual ExpressCompany ExpressCompany { get; set; }

        public string NoteOfCustomer { get; set; }

        public string OutTradeNo { get; set; }

        public string PrepayId { get; set; }

        public DateTime? PrepayIdCreatedTime { get; set; }

        public virtual UserIdentifier GetUserIdentifier()
        {
            return new UserIdentifier(TenantId, UserId);
        }

        public string GetExpressCompanyName()
        {
            if (ExpressCompany == null)
            {
                return null;
            }
            return ExpressCompany.Name;
        }

        public string GetProductInfo()
        {
            string productInfo = "";
            int index = 0;

            foreach(OrderItem orderItem in OrderItems)
            {
                productInfo += orderItem.Specification.GetFullName() + "———" + orderItem.Count;

                if (index > 0)
                {
                    productInfo += "\n";
                }
                index++;
            }
            return productInfo;
        }

        public string GetStatusText()
        {
            return Enum.GetName(typeof(OrderStatus), OrderStatus);  
        }
    }
}
