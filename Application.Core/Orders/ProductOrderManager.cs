using Application.Authorization.Users;
using Application.Customers;
using Application.Expresses;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.SalePriceProviders;
using Application.Orders.SalePriceServices;
using Application.Products;
using Application.ShopCarts;
using Application.Shops;
using Infrastructure.AutoMapper;
using Infrastructure.Configuration;
using Infrastructure.Domain.Repositories;
using Infrastructure.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class ProductOrderManager : OrderManager<ProductOrder, ProductBoughtContext>
    {
        protected IRepository<Product> ProductRepository;
        protected IRepository<Specification> SpecificationRepository;
        public IRepository<CustomerInfo> CustomerInfoRespository;
        public ProductManager ProductManager { get; set; }
        public ShopCartManager ShopCartManager { get; set; }
        public ProductSalePriceService ProductSalePriceService { get; set; }

        public ProductOrderManager(
            IRepository<Product> productRepository,
            IRepository<Specification> specificationRepository,
            IRepository<ProductOrder> orderRepository,
            IRepository<ExpressCompany> expressCompanyRepository,
            IRepository<User, long> userRepository,
            IRepository<CustomerInfo> customerInfoRespository,
            ProductSalePriceProvider productSalePriceProvider
            ) :base(orderRepository, userRepository, expressCompanyRepository, productSalePriceProvider)
        {
            ExpressCompanyRepository = expressCompanyRepository;
            ProductRepository = productRepository;
            SpecificationRepository = specificationRepository;
            CustomerInfoRespository = customerInfoRespository;
        }

        public async Task<bool> CheckBuyPermission()
        {
            bool ShouldBeSpreadForBuy = await SettingManager.GetSettingValueForTenantAsync<bool>(ShopSettings.Order.ShouldHasParentForBuy, InfrastructureSession.TenantId.Value);

            if (ShouldBeSpreadForBuy)
            {
                User user = UserRepository.Get(InfrastructureSession.UserId.Value);

                if (!user.ParentUserId.HasValue)
                {
                    throw new UserFriendlyException(L("YouHasNoParentForBuy"));
                }
            }
            return true;
        }

        public override ProductBoughtContext PreProcessOrderData(ProductBoughtContext boughtContext)
        {
            boughtContext.Order.OrderItems = boughtContext.BoughtItems.MapTo<List<OrderItem>>();
            boughtContext.Order.OrderStatus = OrderStatus.UnPay;
            boughtContext.Order.IsNeedShip = true;
            boughtContext.Order.IsNeedLogistics = boughtContext.IsNeedLogistics;
            boughtContext.Order.NoteOfCustomer = boughtContext.NoteOfCustomer;

            if (boughtContext.CustomerInfoId.HasValue)
            {
                CustomerInfo CustomerInfo = CustomerInfoRespository.Get(boughtContext.CustomerInfoId.Value);
                OrderCustomerInfo OrderCustomerInfo = new OrderCustomerInfo()
                {
                    FullName=CustomerInfo.FullName,
                    PhoneNumber=CustomerInfo.PhoneNumber,
                    Address=CustomerInfo.GetFullAdress()
                };
                boughtContext.Order.OrderCustomerInfo = OrderCustomerInfo;
            }
            return base.PreProcessOrderData(boughtContext);
        }

        public override string BuildTitle(ProductBoughtContext boughtContext)
        {
            string title = "";
            int i = 0;

            foreach (OrderItem orderItem in boughtContext.Order.OrderItems)
            {
                title += orderItem.Specification.Product.Name;

                foreach(SpecificationPropertyValue specificationPropertyValue in orderItem.Specification.PropertyValues)
                {

                    if (i > 0)
                    {
                        title += "&";
                    }
                    title += specificationPropertyValue.Value;
                }
                i++;
            }
            return title;
        }

        public async Task<ProductOrder> CreateOrder(ProductBoughtContext boughtContext)
        {
            foreach(BoughtItem boughtItem in boughtContext.BoughtItems)
            {
                boughtItem.Specification = SpecificationRepository.Get(boughtItem.SpecificationId);
                boughtItem.ProductId = boughtItem.Specification.ProductId;
                ProductManager.CheckStock(boughtItem.Specification, boughtItem.Count);

                if (boughtItem.CartItemId.HasValue)
                {
                    ShopCartManager.RemoveItem(boughtItem.CartItemId.Value);
                }
                DecreaseStockWhen decreaseStockWhen = (DecreaseStockWhen)(Enum.Parse(typeof(DecreaseStockWhen),
                    SettingManager.GetSettingValueForTenant(ShopSettings.General.DecreaseStockWhen,InfrastructureSession.TenantId.Value)));

                if(decreaseStockWhen== DecreaseStockWhen.Create)
                {
                    ProductManager.DecreaseStock(boughtItem.Specification, boughtItem.Count);
                }
            }
            ProductOrder order =await Create(boughtContext);
            return order;
        }
    }
}
