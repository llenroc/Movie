using Application.Distributions;
using Application.ProductCategorys;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Products
{
    public enum ProductStatus
    {
        Off,
        On
    }

    public enum ProductType
    {
        Physical,
        Virtual
    }

    public enum VirtualProductType
    {
        VirtualCard,
        Coupons
    }

    public class Product:FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Content { get; set; }

        public ProductType Type { get; set; }

        public VirtualProductType VirtualProductType { get; set; }

        public string CardName { get; set; }

        public int ShopId { get; set; }

        public int ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public int? TempleteId { get; set; }

        public bool IsVirtual { get; set; } = false;

        public ProductStatus Status { get; set; }

        public bool IsRedirectExternal { get; set; }

        public string ExternalLink { get; set; }

        public string ShareTitle { get; set; }

        public string ShareDescription { get; set; }

        public string SharePicture { get; set; }

        public string MasterQrcode { get; set; }

        public virtual ICollection<SpecificationProperty> SpecificationPropertys { get; set; }

        public virtual ICollection<Specification> Specifications { get; set; }

        public virtual ICollection<Distribution> Distributions { get; set; }

        public string GetPrice()
        {
            decimal minPrice= Specifications.ElementAt(0).Price;
            decimal maxPirce =0;

            foreach (Specification specification in Specifications)
            {
                if (specification.Price < minPrice)
                {
                    minPrice = specification.Price;
                }
                else if (specification.Price > maxPirce)
                {
                    maxPirce = specification.Price;
                }
            }

            if (minPrice == maxPirce)
            {
                return minPrice.ToString();
            }
            else
            {
                return minPrice + "-" + maxPirce;
            }
        }

        public int GetStock()
        {
            int stock = 0;

            if (Specifications != null)
            {
                foreach (Specification specification in Specifications)
                {
                    stock += specification.Stock;
                }
            }
            return stock;
        }

        public int GetSale()
        {
            int sale = 0;

            if (Specifications != null)
            {
                foreach (Specification specification in Specifications)
                {
                    sale += specification.Sale;
                }
            }
            return sale;
        }

        public bool IsAutoShip { get; set; }

        public string GetPicture()
        {
            if (Specifications.Count==0)
            {
                return null;
            }
            return Specifications.ElementAt(0).Picture;
        }
    }
}
