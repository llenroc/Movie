using Application.Orders.Entities;
using Infrastructure.AutoMapper;

namespace Application.Orders.Admins.Dto
{
    [AutoMapFrom(typeof(OrderCustomerInfo))]
    public class OrderCustomerInfoForOrderForExportDto
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
