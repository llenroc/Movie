using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Infrastructure.Application.DTO;
using Application.ProductCategorys.Tenants.Dto;
using Application.Products.Fronts.Dto;

namespace Application.Products.Fronts
{
    public interface IProductForFrontAppService: IApplicationService
    {
        ProductDto GetProduct(ProductGetInput input);

        PagedResultDto<ProductListDto> GetAllOfPage(PagedAndSortedResultRequestDto input);
    }
}
