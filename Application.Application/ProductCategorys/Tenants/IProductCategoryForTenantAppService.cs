using Application.ProductCategorys.Tenants.Dto;
using Application.Products.Tenants.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductCategorys.Tenants
{
    public interface IProductCategoryForTenantAppService : ICrudAppService<ProductCategoryDto>
    {
        Task<ProductCategoryForEditOutput> GetProductCategoryForEdit(NullableIdDto input);

        ProductCategoryDto CreateOrUpdateProductCategory(ProductCategoryCreateOrEditDto input);
    }
}
