using Application.Customers;
using Application.Regions;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomerInfos.Dto
{
    [AutoMap(typeof(CustomerInfo))]
    public class CustomerInfoDto : EntityDto
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string DetailAddress { get; set; }

        public int AddressId { get; set; }

        public AddressDto Address { get; set; }

        public bool IsDefault { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get{
                if (Address != null)
                {
                    string fullAddress = "";

                    BuildFullAddress(ref fullAddress, Address);
                    fullAddress += DetailAddress;
                    return fullAddress;
                }
                else
                {
                    return null;
                }
            }
        }

        private void BuildFullAddress(ref string fullAddress, AddressDto address)
        {
            fullAddress = address.Name + fullAddress;

            if (address.Parent != null)
            {
                BuildFullAddress(ref fullAddress, address.Parent);
            }
        }
    }
}
