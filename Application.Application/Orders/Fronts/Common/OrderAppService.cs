using Application.Orders.Entities;
using Application.Orders.Fronts.Common.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using Infrastructure.Linq.Extensions;
using Infrastructure.UI;
using System.Linq;

namespace Application.Orders.Fronts.Common
{
    public class OrderAppService : CrudAppService<
            Order,
            OrderDto,
            int,
            OrderGetAllInput,
            OrderDto,
            OrderDto, OrderGetInput, OrderGetInput>
        , IOrderAppService
    {
        protected IRepository<Order> OrderBaseRepository;
        public CommonOrderManager OrderManager { get; set; }

        public OrderAppService(IRepository<Order> repository,
            IRepository<Order> orderBaseRepository) : base(repository)
        {
            OrderBaseRepository = orderBaseRepository;
        }

        protected override IQueryable<Order> CreateFilteredQuery(OrderGetAllInput input)
        {
            return Repository.GetAll().Where(model=>model.CreatorUserId==InfrastructureSession.UserId)
                .WhereIf(input.OrderStatus!=null,model=>model.OrderStatus==input.OrderStatus);
        }

        public OrderDto Receive(IdInput input)
        {
            Order order = Repository.Get(input.Id);
            OrderManager.Receive(order);
            return order.MapTo<OrderDto>();
        }

        public void CloseOrder(IdInput input)
        {
            Order order = Repository.Get(input.Id);
            OrderManager.CloseOrder(order);
        }

        public PayOutput GetPayOutput(PayInput input)
        {
            OrderDto order = Get(new OrderGetInput()
            {
                Id=input.Id
            });

            if (order.PaymentStatus == PaymentStatus.Payed)
            {
                throw new UserFriendlyException(L("OrderHasPayed"));
            }
            PayOutput payOutput = new PayOutput()
            {
                Order = order
            };
            return payOutput;
        }
    }
}
