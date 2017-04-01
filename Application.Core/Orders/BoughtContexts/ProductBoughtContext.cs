using Application.Orders.Entities;
using Application.Products;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.BoughtContexts
{
    public class ProductBoughtContext : IBoughtContext<ProductOrder>, ISingletonDependency
    {
        public ProductOrder Order { get; set; }

        public decimal Money { get; set; } = 0;

        public List<BoughtItem> BoughtItems { get; set; }

        public int? CustomerInfoId { get; set; }

        public string NoteOfCustomer { get; set; }

        public int ProductCount
        {
            get
            {
                int count = 0;

                foreach (BoughtItem boughtItem in BoughtItems)
                {
                    count += boughtItem.Count;
                }
                return count;
            }
        }

        public bool IsNeedLogistics
        {
            get
            {
                foreach (BoughtItem boughtItem in BoughtItems)
                {
                    if (boughtItem.Specification.Product.Type == ProductType.Physical)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
