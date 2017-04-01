using Application.Regions.Fronts.Dto;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Regions.Fronts
{
    public class AddressForFrontAppService: IAddressForFrontAppService
    {
        public IRepository<Address> _addressRepository;

        public AddressForFrontAppService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public List<AddressDto> GetAddresses(AddressGetAllInput input)
        {
            List<Address> addresses = _addressRepository.GetAll().Where(model => model.ParentId == input.ParentId).ToList();
            return addresses.MapTo<List<AddressDto>>();
        }
    }
}
