﻿using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System.Collections.Generic;

namespace Application.Products.Tenants.Dto
{
    [AutoMapTo(typeof(Product))]
    public class ProductCreateOrEditInputDto:NullableIdDto
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public int ProductCategoryId { get; set; }

        public int? TempleteId { get; set; }

        public int Sale { get; set; }

        public ProductType Type { get; set; }

        public VirtualProductType VirtualProductType { get; set; }

        public string CardName { get; set; }

        public ProductStatus Status { get; set; }

        public bool IsRedirectExternal { get; set; }

        public string ExternalLink { get; set; }

        public string MasterQrcode { get; set; }

        public string ShareTitle { get; set; }

        public string ShareDescription { get; set; }

        public string SharePicture { get; set; }

        public List<SpecificationPropertyDto> SpecificationPropertys { get; set; }
    }
}
