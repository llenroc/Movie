using Application.Channel.End.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.End
{
    public interface IChannelAgentForEndAppService: ICrudAppService<ChannelAgentDto>
    {
        ChannelAgentForCreateOrEditDto GetChannelAgentForCreateOrEdit(NullableIdDto input);

        ChannelAgentForCreateOrEditDto CreateOrEdit(ChannelAgentForCreateOrEditDto input);
    }
}
