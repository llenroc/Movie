using Application.Orders.Fronts.Common.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;

namespace Application.Orders.Fronts.Common
{
    public interface IOrderAppService : ICrudAppService<
        OrderDto,
        int,
        OrderGetAllInput,
        OrderDto,
        OrderDto, 
        OrderGetInput, 
        OrderGetInput>
    {
        PayOutput GetPayOutput(PayInput input);

        OrderDto Receive(IdInput input);

        void CloseOrder(IdInput input);
    }
}
