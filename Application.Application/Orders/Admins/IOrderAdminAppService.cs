using Application.IO;
using Application.Orders.Admins.Dto;
using Infrastructure.Application.Services;
using System.Threading.Tasks;

namespace Application.Admins.Orders
{
    public interface IOrderAdminAppService : ICrudAppService<
        OrderDto,
        int,
        OrderGetAllInput,
        OrderCreateOrUpdateInput,
        OrderEditInput, 
        OrderGetInput, 
        OrderGetInput>
    {
        Task<FileDto> GetOrdersToExcel();

        Task ChangePayMoney(ChangePayMoneyOrderInput input);

        OrderForEditOutput GetOrderForEditOutput(OrderGetInput input);

        OrderForShipOutput GetOrderForShipOutput(OrderGetInput input);

        Task Ship(ShipOrderInput input);

        Task BathShipFromExcel(BatchShipInput input);

        Task SetAsPayed(OrderGetInput input);
    }
}
