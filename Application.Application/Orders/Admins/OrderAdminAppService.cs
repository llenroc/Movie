using Application.Expresses;
using Application.Expresses.Dto;
using Application.IO;
using Application.Orders;
using Application.Orders.Admins.Dto;
using Application.Orders.Admins.Exporting;
using Application.Orders.Entities;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.BackgroundJobs;
using Infrastructure.Domain.Repositories;
using Infrastructure.Linq.Extensions;
using Infrastructure.Threading;
using Infrastructure.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Application.Admins.Orders
{
    public class OrderAdminAppService:
        CrudAppService<
            Order,
            OrderDto,
            int,
            OrderGetAllInput,
            OrderCreateOrUpdateInput,
            OrderEditInput, 
            OrderGetInput, 
            OrderGetInput>
        , IOrderAdminAppService
    {
        protected IRepository<ExpressCompany> ExpressCompanyRepository;
        public CommonOrderManager OrderManager { get; set; }
        public ExcelHelper ExcelHelper { get; set; }
        public OrderListExcelExporter OrderListExcelExporter { get; set; }
        private readonly IBackgroundJobManager _backgroundJobManager;

        public OrderAdminAppService(
            IBackgroundJobManager backgroundJobManager,
            IRepository<Order> repository,
            IRepository<ExpressCompany> expressCompanyRepository) : base(repository)
        {
            _backgroundJobManager = backgroundJobManager;
            ExpressCompanyRepository = expressCompanyRepository;
        }

        protected override IQueryable<Order> CreateFilteredQuery(OrderGetAllInput input)
        {
            return Repository.GetAll()
                .WhereIf(input.OrderStatus!=null, model=>model.OrderStatus == input.OrderStatus)
                .WhereIf(input.StartTime!=null,model=>model.CreationTime>=input.StartTime)
                .WhereIf(input.EndTime != null, model => model.CreationTime <= input.EndTime)
                .WhereIf(!String.IsNullOrEmpty(input.TrackingNumber),model=>model.TrackingNumber==input.TrackingNumber)
                .WhereIf(!String.IsNullOrEmpty(input.FullName),model=>model.OrderCustomerInfo.FullName==input.FullName)
                .WhereIf(!String.IsNullOrEmpty(input.PhoneNumber),model=>model.OrderCustomerInfo.PhoneNumber==input.PhoneNumber);
        }

        public async Task ChangePayMoney(ChangePayMoneyOrderInput input)
        {
            Order order = Repository.Get(input.Id);
            order.PayMoney = input.PayMoney;
            order.PrepayId = null;
            order.PrepayIdCreatedTime = null;
            await Repository.UpdateAsync(order);
        }

        public async Task<FileDto> GetOrdersToExcel()
        {
            var orders = Repository.GetAll().Where(model => model.IsNeedShip && model.OrderStatus == OrderStatus.UnShip).ToList();

            if (orders.Count() == 0)
            {
                throw new UserFriendlyException(L("ThereIsNoOrderToShip"));
            }
            var ordersForExport= orders.MapTo<List<OrderForExportDto>>();
            return OrderListExcelExporter.ExportToFile(ordersForExport);
        }

        public async Task BathShipFromExcel(BatchShipInput input)
        {
            AsyncHelper.RunSync(async () =>
            {
                List<OrderOfBatchShipInput> orderOfBatchShipInputs = ExcelHelper.LoadFromExcel<OrderOfBatchShipInput>(HttpContext.Current.Server.MapPath(input.FilePath)).ToList();

                foreach (OrderOfBatchShipInput orderOfBatchShipInput in orderOfBatchShipInputs)
                {
                    await OrderManager.Ship(orderOfBatchShipInput.Number, orderOfBatchShipInput.ExpressCompany, orderOfBatchShipInput.TrackingNumber);
                }
            });
            //_backgroundJobManager.Enqueue<BatchShipJob, BatchShipJobArgs>(new BatchShipJobArgs(HttpContext.Current.Server.MapPath(input.FilePath)));
        }

        public OrderForEditOutput GetOrderForEditOutput(OrderGetInput input)
        {
            OrderForEditOutput orderForEditOutput = new OrderForEditOutput()
            {
                Order=Repository.Get(input.Id).MapTo<OrderDto>()
            };
            return orderForEditOutput;
        }

        public OrderForShipOutput GetOrderForShipOutput(OrderGetInput input)
        {
            OrderForShipOutput orderForEditOutput = new OrderForShipOutput()
            {
                Order = Repository.Get(input.Id).MapTo<OrderDto>(),
                ExpressCompanys = ExpressCompanyRepository.GetAll().MapTo<List<ExpressCompanyDto>>()
            };
            return orderForEditOutput;
        }

        public async Task Ship(ShipOrderInput input)
        {
            Order order = Repository.Get(input.OrderId);

            try
            {
                await OrderManager.ShipAsync(order, false, input.ExpressInfo);
            }
            catch(Exception exception)
            {
                throw new UserFriendlyException(exception.Message);
            }
        }

        public async Task SetAsPayed(OrderGetInput input)
        {
            Order order = Repository.Get(input.Id);
            await OrderManager.PayCallback(order,Wallets.Entities.PayType.WeChat);
        }
    }
}
