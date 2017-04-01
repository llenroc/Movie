using Application.Products;
using Infrastructure.Domain.Entities.Auditing;

namespace Application.Orders.Entities
{
    public class OrderItem : AuditedEntity
    {
        public int OrderId { get; set; }

        public virtual ProductOrder Order { get;set;}

        public int SpecificationId { get; set; }

        public int? CartItemId { get; set; }

        public int Count { get; private set; }

        public decimal Price { get; set; }

        public decimal Money { get; set; }

        public ShipStatus ShipStatus { get; set; }

        public virtual Specification Specification { get;set;}
    }
}
