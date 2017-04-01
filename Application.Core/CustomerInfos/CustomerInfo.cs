using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using Application.Regions;
using System.ComponentModel.DataAnnotations;

namespace Application.Customers
{
    public class CustomerInfo : FullAuditedEntity,IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string DetailAddress { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address{get;set;}

        public bool IsDefault { get; set; }

        public string GetFullAdress()
        {
            if (Address == null)
            {
                return DetailAddress;
            }
            else
            {
                return Address.GetFullAddress() + DetailAddress;
            }
        }
    }
}
