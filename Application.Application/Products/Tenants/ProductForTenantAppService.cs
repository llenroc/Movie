using Application.Distributions;
using Application.ProductCategorys;
using Application.ProductCategorys.Tenants.Dto;
using Application.Products.Tenants.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Application.Products.Tenants
{
    public class ProductForTenantAppService:CrudAppService<Product,ProductDto>, IProductForTenantAppService
    {
        public ProductManager ProductManager { get; set; }
        private IRepository<ProductCategory> _productCategoryRespository;
        private IRepository<SpecificationProperty> _specificationPropertyRespository;
        private IRepository<SpecificationPropertyValue> _specificationPropertyValueRespository;
        private IRepository<Distribution> _distributionRespository;
        private IRepository<Specification> _specificationRespository;

        public ProductForTenantAppService(
            IRepository<Product> respository, 
            IRepository<ProductCategory> productCategoryRespository,
            IRepository<SpecificationProperty> specificationPropertyRespository,
            IRepository<SpecificationPropertyValue> specificationPropertyValueRespository,
            IRepository<Distribution> distributionRespository,
            IRepository<Specification> specificationRespository) 
            :base(respository)
        {
            _productCategoryRespository = productCategoryRespository;
            _specificationPropertyRespository = specificationPropertyRespository;
            _specificationPropertyValueRespository = specificationPropertyValueRespository;
            _distributionRespository = distributionRespository;
            _specificationRespository = specificationRespository;
        }

        public List<ProductCategoryDto> GetProductCategorys()
        {
            var entities = _productCategoryRespository.GetAll().ToList();
            return entities.Select(entity=>entity.MapTo<ProductCategoryDto>()).ToList();
        }

        private List<SpecificationPropertyDto> GetSpecificationProperty()
        {
            var entities = _specificationPropertyRespository.GetAll().ToList();
            return entities.Select(entity => entity.MapTo<SpecificationPropertyDto>()).ToList();
        }

        public ProductDto CreateOrEdit(ProductCreateOrEditInput input)
        {
            if (input.Product.Id.HasValue)
            {
                CheckUpdatePermission();

                var entity = GetEntityById(input.Product.Id.Value);
                ObjectMapper.Map(input.Product, entity);

                List<SpecificationProperty> specificationPropertys = new List<SpecificationProperty>();

                foreach (SpecificationProperty specificationProperty in entity.SpecificationPropertys)
                {
                    specificationPropertys.Add(_specificationPropertyRespository.Get(specificationProperty.Id));
                }
                entity.SpecificationPropertys = specificationPropertys;
                CurrentUnitOfWork.SaveChanges();

                SetSpecifications(entity, input.Specifications);
                SetDistributions(entity,input.Distributions);
                return MapToEntityDto(entity);
            }
            else
            {
                CheckCreatePermission();
                var entity = input.Product.MapTo<Product>();

                List<SpecificationProperty> specificationPropertys = new List<SpecificationProperty>();

                foreach (SpecificationProperty specificationProperty in entity.SpecificationPropertys)
                {
                    specificationPropertys.Add(_specificationPropertyRespository.Get(specificationProperty.Id));
                }
                entity.SpecificationPropertys = specificationPropertys;

                Repository.Insert(entity);
                CurrentUnitOfWork.SaveChanges();

                SetSpecifications(entity, input.Specifications);
                SetDistributions(entity, input.Distributions);
                return MapToEntityDto(entity);
            }
        }

        public void OnProduct(IdInput input)
        {
            Product product = Repository.Get(input.Id);
            product.Status = ProductStatus.On;
            Repository.Update(product);
        }

        public void OffProduct(IdInput input)
        {
            Product product = Repository.Get(input.Id);
            product.Status = ProductStatus.Off;
            Repository.Update(product);
        }

        public void RemoveDistribution(IdInput input)
        {
            var distribution = _distributionRespository.Get(input.Id);
            _distributionRespository.Delete(distribution);
        }

        public void RemoveSpecification(IdInput input)
        {
            var specification = _specificationRespository.Get(input.Id);
            _specificationRespository.Delete(specification);
        }

        private void SetSpecifications(Product product, List<SpecificationForCreateOrEditInput> specifications)
        {
            foreach (var specification in specifications)
            {
                if (specification.Id.HasValue)
                {
                    var entity = _specificationRespository.Get(specification.Id.Value);
                    ObjectMapper.Map(specification, entity);
                    CurrentUnitOfWork.SaveChanges();
                    SetSpecificationPropertyValue(entity, specification.PropertyValues);
                }
                else
                {
                    var entity = specification.MapTo<Specification>();
                    entity.ProductId = product.Id;
                    _specificationRespository.Insert(entity);
                    CurrentUnitOfWork.SaveChanges();
                    SetSpecificationPropertyValue(entity, specification.PropertyValues);
                }
            }
        }

        private void SetSpecificationPropertyValue(Specification specification, 
            List<SpecificationPropertyValueForCreateOrEditDto> propertyValues)
        {
            foreach (SpecificationPropertyValueForCreateOrEditDto propertyValue in propertyValues)
            {
                if (propertyValue.Id.HasValue)
                {
                    var entity = _specificationPropertyValueRespository.Get(propertyValue.Id.Value);
                    ObjectMapper.Map(propertyValue, entity);
                    CurrentUnitOfWork.SaveChanges();
                }
                else
                {
                    var entity = propertyValue.MapTo<SpecificationPropertyValue>();
                    entity.SpecificationId = specification.Id;
                    _specificationPropertyValueRespository.Insert(entity);
                    CurrentUnitOfWork.SaveChanges();
                }
            }
        }

        private void SetDistributions(Product product,List<DistributionCreateOrEditInput> distributions)
        {
            if (distributions == null)
            {
                return;
            }

            foreach (var distribution in distributions)
            {
                if (distribution.Id.HasValue)
                {
                    var entity = _distributionRespository.Get(distribution.Id.Value);
                    ObjectMapper.Map(distribution, entity);
                    CurrentUnitOfWork.SaveChanges();
                }
                else
                {
                    var entity = distribution.MapTo<Distribution>();
                    entity.ProductId = product.Id;
                    _distributionRespository.Insert(entity);
                    CurrentUnitOfWork.SaveChanges();
                }
            }
        }

        public ProductForCreateOrEditOutput GetProductForCreateOrEdit(NullableIdDto input)
        {
            ProductForCreateOrEditOutputDto product = new ProductForCreateOrEditOutputDto();
            ProductForCreateOrEditOutput productForCreateOutput = new ProductForCreateOrEditOutput()
            {
                SpecificationPropertys = GetSpecificationProperty(),
                ProductCategorys = GetProductCategorys(),
                Product = product
            };

            if (input.Id.HasValue)
            {
                productForCreateOutput.Product = Repository.Get(input.Id.Value).MapTo<ProductForCreateOrEditOutputDto>();
                productForCreateOutput.Specifications=_specificationRespository.GetAll()
                    .Where(model => model.ProductId == input.Id.Value).ToList()
                    .Select(entity=>ObjectMapper.Map<SpecificationDto>(entity)).ToList();
                productForCreateOutput.Distributions = _distributionRespository.GetAll()
                    .Where(model => model.ProductId == input.Id.Value).ToList()
                    .Select(entity => ObjectMapper.Map<DistributionDto>(entity)).ToList();
            }
            return productForCreateOutput;
        }
    }
}
