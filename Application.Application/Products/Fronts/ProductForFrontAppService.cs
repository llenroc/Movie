using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Application.Products.Fronts.Dto;
using Infrastructure.Domain.Repositories;
using Application.ProductCategorys;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System.Linq.Dynamic;
using Infrastructure.Linq.Extensions;
using Infrastructure.UI;

namespace Application.Products.Fronts
{
    public class ProductForFrontAppService:ApplicationAppServiceBase, IProductForFrontAppService
    {
        private IRepository<Product> _productRespository;
        private IRepository<ProductCategory> _productCategoryRespository;
        private IRepository<SpecificationProperty> _specificationPropertyRespository;

        public ProductForFrontAppService(IRepository<Product> productRespository, 
            IRepository<ProductCategory> productCategoryRespository,
            IRepository<SpecificationProperty> specificationPropertyRespository)
        {
            _productRespository = productRespository;
            _productCategoryRespository = productCategoryRespository;
            _specificationPropertyRespository = specificationPropertyRespository;
        }

        private SpecificationPropertyGroupList GetSpecificationPropertyGroups(Product product)
        {
            SpecificationPropertyGroupList specificationPropertyGroups = new SpecificationPropertyGroupList();

            foreach (Specification specification in product.Specifications)
            {
                foreach (SpecificationPropertyValue specificationPropertyValue in specification.PropertyValues)
                {
                    PropertyValueDto specificationPropertyValueDto = specificationPropertyValue.MapTo<PropertyValueDto>();

                    if (specificationPropertyGroups.HasSpecificationProperty(specificationPropertyValue.SpecificationProperty))
                    {
                        var specificationPropertyGroup = specificationPropertyGroups.GetFromSpecificationProperty(specificationPropertyValue.SpecificationProperty);

                        if (!HasPropertyValueDto(specificationPropertyGroup.SpecificationPropertyValues,specificationPropertyValueDto))
                        {
                            specificationPropertyGroup.SpecificationPropertyValues.Add(specificationPropertyValueDto);
                        }
                    }
                    else
                    {
                        var specificationPropertyGroup = new SpecificationPropertyGroup()
                        {
                            SpecificationProperty = specificationPropertyValue.SpecificationProperty.MapTo<SpecificationPropertyDto>(),
                            SpecificationPropertyValues = new List<PropertyValueDto>()
                        };
                        specificationPropertyGroup.SpecificationPropertyValues.Add(specificationPropertyValueDto);
                        specificationPropertyGroups.Add(specificationPropertyGroup);
                    }
                }
            }
            return specificationPropertyGroups;
        }

        private bool HasPropertyValueDto(List<PropertyValueDto> list, PropertyValueDto propertyValueDto)
        {
            foreach (PropertyValueDto propertyValueDtoItem in list)
            {
                if (propertyValueDtoItem.Value == propertyValueDto.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public ProductDto GetProduct(ProductGetInput input)
        {
            Product product = _productRespository.Get(input.Id);

            if (product.Status == ProductStatus.Off)
            {
                throw new UserFriendlyException(L("TheProductIsOff"));
            }
            ProductDto productDto= product.MapTo<ProductDto>();

            SpecificationPropertyGroupList SpecificationPropertyGroupList = GetSpecificationPropertyGroups(product);
            productDto.SpecificationPropertyGroups = SpecificationPropertyGroupList;
            return productDto;
        }

        public PagedResultDto<ProductListDto> GetAllOfPage(PagedAndSortedResultRequestDto input)
        {
            var query=_productRespository.GetAll().Where(model=>model.Status==ProductStatus.On);
            var totalCount = query.Count();

            if (!string.IsNullOrEmpty(input.Sorting))
            {
                query.OrderBy(input.Sorting);
            }
            query.PageBy((input.PageIndex - 1) * input.PageSize, input.PageSize);

            var products = query.ToList().MapTo<List<ProductListDto>>();

            return new PagedResultDto<ProductListDto>(
                totalCount,
                input.PageIndex,
                input.PageSize,
                products
            );
        }
    }
}
