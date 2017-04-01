using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Fronts.Dto
{
    [AutoMap(typeof(Specification))]
    public class SpecificationDto: FullAuditedEntityDto
    {
        public string Picture { get; set; }

        public int Stock { get; set; }

        public ProductStatus Status { get; set; }

        public string Number { get; set; }

        public string Barcode { get; set; }

        public decimal Price { get; set; }

        public List<SpecificationPropertyValueDto> PropertyValues { get; set; }
    }
}
