using Application.Authorization.Users;
using Application.Orders;
using Application.Orders.Entities;
using Infrastructure.Dependency;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Threading.BackgroundWorkers;
using Infrastructure.Threading.Timers;
using Infrastructure.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BackgroundWorker
{
    public class MakeOvertimeOrdersDeletedBackgroundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Order> _orderRepository;
        private CommonOrderManager _orderManager;

        public MakeOvertimeOrdersDeletedBackgroundWorker(
            CommonOrderManager orderManager,
            InfrastructureTimer timer, 
            IRepository<Order> orderRepository)
        : base(timer)
        {
            _orderManager = orderManager;
            _orderRepository = orderRepository;
            Timer.Period = 7200000; 
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(DataFilters.MustHaveTenant))
            {
                DateTime dateTime = DateTime.Now.AddHours(-2);
                var overTimeOrders = _orderRepository.GetAllList(model=>model.OrderStatus==OrderStatus.UnPay
                &&model.CreationTime< dateTime);

                foreach (var order in overTimeOrders)
                {
                    _orderManager.CloseOrder(order);
                }
            }
        }
    }
}
