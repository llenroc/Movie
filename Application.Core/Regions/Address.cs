using Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Regions
{
    public class Address:Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public virtual Address Parent { get; set; }

        public int Level { get; set; }

        public int Sort { get; set; }

        public string EnglishName { get; set; }

        public string ShortEnglishName { get; set; }

        public string GetFullAddress()
        {
            string fullAddress = "";
            GetFullAddressRecursion(this,ref fullAddress);
            return fullAddress;
        }

        private void GetFullAddressRecursion(Address address,ref string fullAddress)
        {
            fullAddress = address.Name + fullAddress;

            if (address.Parent != null)
            {
                GetFullAddressRecursion(address.Parent, ref fullAddress);
            }
        }
    }
}
