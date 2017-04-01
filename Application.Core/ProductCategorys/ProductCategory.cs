using Infrastructure.Domain.Entities.Auditing;
using Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductCategorys
{
    public class ProductCategory : FullAuditedEntity,IMustHaveTenant
    {
        public int TenantId { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int? ParentId { get; set; }

        public virtual ProductCategory Parent { get; set; }
    }
}
