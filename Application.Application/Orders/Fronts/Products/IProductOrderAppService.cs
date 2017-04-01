using Application.Orders.Fronts.Dto;
using Application.Orders.Fronts.Products.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.Products
{
    public interface IProductOrderAppService : ICrudAppService<
        OrderDto,
        int,
        OrderGetAllInput,
        OrderDto,
        OrderDto, 
        OrderGetInput, 
        OrderGetInput>
    {
        PayOutput GetPayOutput(PayInput input);

        List<BoughtItemOutput> GetBoughtItemsFromShopCart();

        OrderConfirmOutput ConfirmOrder(OrderConfirmInput input);

        Task<OrderDto> CreateOrder(OrderCreateInput input);

        OrderDto Receive(IdInput input);
    }
}
