using Application.Orders.Entities;

namespace Application.Orders.Admins.Dto
{
    public class ShipOrderInput
    {
        public int OrderId { get; set; }

        public ExpressInfo ExpressInfo { get; set; } = null;
    }
}
