using Infrastructure.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Products
{
    public class ProductComment:FullAuditedEntity
    {
        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public int OrderItemId { get; set; }

        [MaxLength(1024)]
        public string Content { get; set; }

        public List<string> Pictures { get; set; }
    }
}
