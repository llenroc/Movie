using Application.Orders.BoughtContexts;
using Application.Products.Fronts.Dto;
using Infrastructure.AutoMapper;

namespace Application.Orders.Fronts.Products.Dto
{
    [AutoMapFrom(typeof(BoughtItem))]
    public class BoughtItemOutput
    {
        public int SpecificationId { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public decimal Money { get; set; }

        public int? CartItemId { get; set; }

        public ProductForOrderConfirmOutput Product { get; set; }

        public SpecificationForBoughtItemDto Specification { get; set; }
    }
}
