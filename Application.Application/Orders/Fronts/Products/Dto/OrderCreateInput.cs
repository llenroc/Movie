using Application.Orders.BoughtContexts;
using Infrastructure.AutoMapper;
using System.Collections.Generic;

namespace Application.Orders.Fronts.Products.Dto
{
    [AutoMapTo(typeof(ProductBoughtContext))]
    public class OrderCreateInput
    {
        public List<BoughtItemInput> BoughtItems { get; set; }

        public int? CustomerInfoId { get; set; }

        public string NoteOfCustomer { get; set; }
    }
}
