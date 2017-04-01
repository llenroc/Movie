using Application.Authorization.Front.Dto;
using Application.Channel.Front.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;

namespace Application.Channel.Front
{
    public interface IChannelAgencyForFrontAppService: IApplicationService
    {
        ChannelAgencyDto GetChannelAgency();

        MyChannelAgentInfo GetMyChannelAgentInfo();

        PagedResultDto<CommonUserForProfileDto> GetCustomersOfChannelAgency(CustomerGetAllInput input);
    }
}
