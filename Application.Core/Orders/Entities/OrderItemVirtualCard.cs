using Infrastructure.Domain.Entities.Auditing;

namespace Application.Orders.Entities
{
    public class OrderItemVirtualCard:AuditedEntity
    {
        public int OrderId { get; set; }

        public int OrderItemId { get; set; }

        public int VirtualCardId { get; set; }

        public long UserId { get; set; }
    }
}
