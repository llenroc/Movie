using Application.Regions.Fronts.Dto;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Regions.Fronts
{
    public interface IAddressForFrontAppService:IApplicationService
    {
        List<AddressDto> GetAddresses(AddressGetAllInput input);
    }
}
