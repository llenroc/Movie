using Application.Authorization.Users;
using Application.Channel.ChananlAgencys;
using Application.Expresses;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.Events;
using Application.Orders.Notifications;
using Application.Orders.NumberProviders;
using Application.Orders.SalePriceProviders;
using Application.Shops;
using Application.Wallets.Entities;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Runtime.Session;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class OrderManager<TOrder, TBoughtContext> : ApplicationDomainServiceBase, IOrderManager<TOrder,TBoughtContext>
        where TOrder : Order, new() 
        where TBoughtContext:IBoughtContext<TOrder>
    {
        protected IRepository<TOrder> OrderRepository;
        protected IRepository<ExpressCompany> ExpressCompanyRepository;

        protected IRepository<User, long> UserRepository;

        public IInfrastructureSession Session { get; set; }

        public INumberProvider NumberProvider { get; set; }

        public ChannelAgencyManager ChannelAgencyManager { get; set; }

        public ShipService ShipService { get; set; }

        public IOrderNotifier OrderNotifier { get; set; }

        public ISalePriceProvider<TBoughtContext> SalePriceProvider { get; set; }

        public OrderManager(IRepository<TOrder> orderRepository, 
            IRepository<User, long> userRepository,
            IRepository<ExpressCompany> expressCompanyRepository,
            ISalePriceProvider<TBoughtContext> salePriceProvider
            )
        {
            OrderRepository = orderRepository;
            UserRepository = userRepository;
            ExpressCompanyRepository = expressCompanyRepository;
            NumberProvider = new DefaultNumberProvider();
            SalePriceProvider = salePriceProvider;
        }

        [UnitOfWork]
        public async Task<TOrder> Create(TBoughtContext boughtContext)
        {
            boughtContext.Order = CreateBaseOrderModel();
            ProcessData(boughtContext);
            OrderRepository.Insert(boughtContext.Order);
            CurrentUnitOfWork.SaveChanges();

            EventBus.Trigger(new OrderCreatedEventData(boughtContext.Order,boughtContext.ProductCount));
            return boughtContext.Order;
        }

        public virtual string BuildTitle(TBoughtContext boughtContext)
        {
            return L("DefaultTitle");
        }

        public virtual TBoughtContext PreProcessOrderData(TBoughtContext boughtContext)
        {
            return boughtContext;
        }

        public virtual TBoughtContext ProcessData(TBoughtContext boughtContext)
        {
            SalePriceProvider.Caculate(boughtContext);
            PreProcessOrderData(boughtContext);

            boughtContext.Order.Number = NumberProvider.BuildNumber();
            boughtContext.Order.Title = BuildTitle(boughtContext);
            
            boughtContext.Order.Money = boughtContext.Money;
            boughtContext.Order.PayMoney = boughtContext.Money;
          
            return boughtContext;
        }

        public TOrder CreateBaseOrderModel()
        {
            TOrder order = new TOrder()
            {
                UserId= Session.UserId.Value,
                PaymentStatus=PaymentStatus.ToPay,
                RefundStatus=RefundStatus.ToRefund,
                OrderStatus=OrderStatus.UnPay
            };
            return order;
        }

        [UnitOfWork]
        public TOrder GetOrderFromNumber(string orderNumber)
        {
            using (CurrentUnitOfWork.DisableFilter(DataFilters.MustHaveTenant))
            {
                TOrder order = OrderRepository.FirstOrDefault(model => model.Number == orderNumber);
                return order;
            }
        }

        [UnitOfWork]
        public async Task<TOrder> PayCallback(string orderNumber, PayType payType)
        {
            using (CurrentUnitOfWork.DisableFilter(DataFilters.MustHaveTenant,DataFilters.MayHaveTenant))
            {
                TOrder order = OrderRepository.GetAll().Where(model => model.Number == orderNumber).FirstOrDefault();
                await PayCallback(order, payType);
                return order;
            }
        }

        [UnitOfWork]
        public async Task<TOrder> PayCallback(TOrder order,PayType payType)
        {
            if (order.PaymentStatus == PaymentStatus.Payed)
            {
                throw new InfrastructureException(L("OrderHasPayed"));
            }
            order.PayType = payType;
            order.PaymentStatus = PaymentStatus.Payed;
            order.OrderStatus = OrderStatus.UnShip;
            order.PaymentDatetime = DateTime.Now;
            OrderRepository.Update(order);
            CurrentUnitOfWork.SaveChanges();

            EventBus.Trigger(new OrderPayedEventData(order));

            await OrderNotifier.OrderPayed(order);
            return order;
        }

        [UnitOfWork]
        public async Task Refund(Order order,long refundFee)
        {
            order.RefundStatus = RefundStatus.Refunded;
            
        }

        public async Task Ship(string orderNumber,string expressCompanyName,string trackingNumber)
        {
            using (CurrentUnitOfWork.DisableFilter(DataFilters.MustHaveTenant,DataFilters.MayHaveTenant))
            {
                if (String.IsNullOrEmpty(orderNumber) || String.IsNullOrEmpty(expressCompanyName) || String.IsNullOrEmpty(trackingNumber))
                {
                    return;
                }
                Order order = OrderRepository.GetAll().Where(model => model.Number == orderNumber).FirstOrDefault();

                if (order == null)
                {
                    return;
                }
                ExpressCompany expressCompany = ExpressCompanyRepository.GetAll().Where(model => model.Name == expressCompanyName).FirstOrDefault();

                if (expressCompany == null)
                {
                    return;
                }
                await ShipAsync(order,false, new ExpressInfo(expressCompany.Id, trackingNumber));
            }
        }

        public async Task<Order> ShipAsync(Order order,bool autoShip=false, ExpressInfo expressInfo = null)
        {
            if (!order.IsNeedShip)
            {
                throw new InfrastructureException(L("NoNeedShip"));
            }

            if (order.ShipStatus != ShipStatus.UnShip)
            {
                throw new InfrastructureException(L("OrderHasShiped"));
            }
            ShipService.Ship(order, autoShip, expressInfo );

            if (expressInfo != null)
            {
                order.ExpressCompanyId = expressInfo.ExpressCompanyId;
                order.TrackingNumber = expressInfo.TrackingNumber;
            }
            OrderRepository.Update(order as TOrder);
            CurrentUnitOfWork.SaveChanges();
            return order;
        }

        public bool CheckShipProgressAndCompleteShip(Order order, ExpressInfo expreeInfo = null)
        {
            if (order.OrderItems.Count > 0)
            {
                foreach(OrderItem orderItem in order.OrderItems)
                {
                    if (orderItem.ShipStatus == ShipStatus.UnShip)
                    {
                        return false;
                    }
                }
            }
            CompleteShip(order, expreeInfo);
            return true;
        }

        public TOrder Receive(TOrder order)
        {
            order.ShipStatus = ShipStatus.Received;
            order.OrderStatus = OrderStatus.Received;
            OrderRepository.Update(order);
            EventBus.Trigger(new OrderReceivedEventData(order));
            return order;
        }

        [UnitOfWork]
        public void CloseOrder(Order order)
        {
            DecreaseStockWhen DecreaseStockWhen = (DecreaseStockWhen)(Enum.Parse(typeof(DecreaseStockWhen),
                SettingManager.GetSettingValueForTenant(ShopSettings.General.DecreaseStockWhen, order.TenantId)));

            order.OrderStatus = OrderStatus.Close;

            //Increase stock
            if (DecreaseStockWhen == DecreaseStockWhen.Create)
            {
                foreach (OrderItem orderItem in order.OrderItems)
                {
                    orderItem.Specification.Stock += orderItem.Count;
                }
            }
            CurrentUnitOfWork.SaveChanges();

            if (order is ChannelAgencyApplyOrder)
            {
                ChannelAgencyManager.DeleteChannelAgencyApply(order.Id);
            }
        }

        [UnitOfWork]
        public Order CompleteShip(Order order, ExpressInfo expreeInfo = null)
        {
            order.ShipStatus = ShipStatus.Shipping;
            order.OrderStatus = OrderStatus.Shiped;
            CurrentUnitOfWork.SaveChanges();
            EventBus.Trigger(new OrderShipedEventData(order,expreeInfo));
            return order;
        }

        [UnitOfWork]
        public Order CompleteOrder(Order order)
        {
            order.OrderStatus = OrderStatus.Completed;
            CurrentUnitOfWork.SaveChanges();
            return order;
        }
    }
}
