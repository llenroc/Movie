using Application.Orders.BoughtContexts;
using Application.Orders.SalePriceServices;
using Infrastructure.Dependency;

namespace Application.Orders.SalePriceProviders
{
    public class ProductSalePriceProvider: ISalePriceProvider<ProductBoughtContext>,ISingletonDependency
    {
        public ProductSalePriceService ProductSalePriceService { get; set; }
        public ProductBoughtContext Caculate(ProductBoughtContext boughtContext)
        {
            ProductSalePriceService.Calculate(boughtContext);
            return boughtContext;
        }
    }
}
