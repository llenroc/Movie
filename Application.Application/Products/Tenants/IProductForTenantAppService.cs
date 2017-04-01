using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Application.Products.Tenants.Dto;
using Infrastructure.Application.DTO;
using Application.ProductCategorys.Tenants.Dto;

namespace Application.Products.Tenants
{
    public interface IProductForTenantAppService: ICrudAppService<ProductDto>
    {
        ProductForCreateOrEditOutput GetProductForCreateOrEdit(NullableIdDto input);

        ProductDto CreateOrEdit(ProductCreateOrEditInput input);

        void RemoveDistribution(IdInput input);

        void OnProduct(IdInput input);

        void OffProduct(IdInput input);

        void RemoveSpecification(IdInput input);
    }
}
