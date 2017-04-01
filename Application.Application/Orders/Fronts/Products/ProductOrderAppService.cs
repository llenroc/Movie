using Application.Channel.ChananlAgencys;
using Application.CustomerInfos;
using Application.CustomerInfos.Dto;
using Application.Customers;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.Fronts.Dto;
using Application.Orders.Fronts.Products.Dto;
using Application.Orders.SalePriceServices;
using Application.Products;
using Application.ShopCarts;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using Infrastructure.Linq.Extensions;
using Infrastructure.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.Products
{
    public class ProductOrderAppService : CrudAppService<
            ProductOrder,
            OrderDto,
            int,
            OrderGetAllInput,
            OrderDto,
            OrderDto, OrderGetInput, OrderGetInput>
        , IProductOrderAppService
    {
        protected IRepository<Order> OrderBaseRepository;
        protected IRepository<Product> ProductRepository;
        protected IRepository<Specification> SpecificationRepository;
        protected IRepository<CustomerInfo> CustomerInfoRepository;
        public ProductOrderManager OrderManager { get; set; }
        public ProductSalePriceService ProductSalePriceService { get; set; }
        public CustomerInfoManager CustomerInfoManager { get; set; }
        public ChannelAgencyManager ChannelAgencyManager { get; set; }
        public ShopCartManager ShopCartManager { get; set; }

        public ProductOrderAppService(IRepository<ProductOrder> repository,
            IRepository<Order> orderBaseRepository,
            IRepository<Product> productRepository,
            IRepository<Specification> specificationRepository,
            IRepository<CustomerInfo> customerInfoRepository) : base(repository)
        {
            OrderBaseRepository = orderBaseRepository;
            ProductRepository = productRepository;
            SpecificationRepository = specificationRepository;
            CustomerInfoRepository = customerInfoRepository;
        }

        protected override IQueryable<ProductOrder> CreateFilteredQuery(OrderGetAllInput input)
        {
            return Repository.GetAll().Where(model=>model.CreatorUserId==InfrastructureSession.UserId)
                .WhereIf(input.OrderStatus!=null,model=>model.OrderStatus==input.OrderStatus);
        }

        public List<BoughtItemOutput> GetBoughtItemsFromShopCart()
        {
            ShopCart ShopCart = ShopCartManager.GetShopCart(InfrastructureSession.UserId.Value);
            List<BoughtItemOutput> boughtItems = new List<BoughtItemOutput>();

            foreach (ShopCartItem shopCartItem in ShopCart.ShopCartItems)
            {
                boughtItems.Add(new BoughtItemOutput()
                {
                    SpecificationId=shopCartItem.SpecificationId,
                    Count=shopCartItem.Count,
                    CartItemId=shopCartItem.Id,
                });
            }
            return boughtItems;
        }

        public OrderConfirmOutput ConfirmOrder(OrderConfirmInput input)
        {
            ProductBoughtContext boughtContext = input.MapTo<ProductBoughtContext>();

            foreach (BoughtItem boughtItem in boughtContext.BoughtItems)
            {
                boughtItem.UserId = InfrastructureSession.UserId.Value;
                boughtItem.Specification = SpecificationRepository.Get(boughtItem.SpecificationId);
            }
            ProductSalePriceService.Calculate(boughtContext);

            OrderConfirmOutput orderConfirmOutput = boughtContext.MapTo<OrderConfirmOutput>();

            if (input.CustomerInfoId.HasValue)
            {
                orderConfirmOutput.CustomerInfo = CustomerInfoRepository.Get(input.CustomerInfoId.Value).MapTo<CustomerInfoDto>();
            }
            else
            {
                orderConfirmOutput.CustomerInfo = CustomerInfoManager.GetDefaultCustomerInfo(InfrastructureSession.UserId.Value).MapTo<CustomerInfoDto>();
            }
            return orderConfirmOutput;
        }

        public async Task<OrderDto> CreateOrder(OrderCreateInput input)
        {
            await OrderManager.CheckBuyPermission();
            ProductBoughtContext boughtContext = input.MapTo<ProductBoughtContext>();
            ProductOrder order=await OrderManager.CreateOrder(boughtContext);
            return order.MapTo<OrderDto>();
        }

        public OrderDto Receive(IdInput input)
        {
            ProductOrder order = Repository.Get(input.Id);
            OrderManager.Receive(order);
            return order.MapTo<OrderDto>();
        }

        public PayOutput GetPayOutput(PayInput input)
        {
            OrderDto order = Get(new OrderGetInput()
            {
                Id=input.Id
            });

            if (order.PaymentStatus == PaymentStatus.Payed)
            {
                throw new UserFriendlyException("the order has payed!");
            }
            PayOutput payOutput = new PayOutput()
            {
                Order = order
            };
            return payOutput;
        }
    }
}
