using Application.ProductCategorys.Tenants.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductCategorys.Tenants
{
    public class ProductCategoryForFrontAppService :IApplicationService, IProductCategoryForFrontAppService
    {
        private IRepository<ProductCategory> _productRespository;

        public ProductCategoryForFrontAppService(IRepository<ProductCategory> productRespository) 
        {
            _productRespository = productRespository;
        }
    }
}
