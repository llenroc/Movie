using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.DTO;
using System.ComponentModel.DataAnnotations;
using Infrastructure.AutoMapper;

namespace Application.Products.Fronts.Dto
{
    [AutoMap(typeof(Product))]
    public class ProductDto:EntityDto
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public int ShopId { get; set; }

        public int ProductCategoryId { get; set; }

        public int? TempleteId { get; set; }

        public string Price { get; set; }

        public int Sale { get; set; }

        public int Stock { get; set; }

        public ProductType Type { get; set; }

        public ProductStatus Status { get; set; }

        public bool IsRedirectExternal { get; set; }

        public string ExternalLink { get; set; }

        public string ShareTitle { get; set; }

        public string ShareDescription { get; set; }

        public string SharePicture { get; set; }

        public List<SpecificationPropertyDto> SpecificationPropertys { get; set; }

        public List<SpecificationDto> Specifications { get; set; }

        public SpecificationPropertyGroupList SpecificationPropertyGroups { get; set; }
    }
}
