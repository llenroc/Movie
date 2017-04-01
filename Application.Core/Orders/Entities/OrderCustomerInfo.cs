using Infrastructure.Domain.Entities;

namespace Application.Orders.Entities
{
    public class OrderCustomerInfo:Entity
    {
        public virtual Order Order { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
