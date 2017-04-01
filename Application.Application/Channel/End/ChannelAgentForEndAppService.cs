using Application.Channel.ChannelAgents;
using Application.Channel.End.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;

namespace Application.Channel.End
{
    public class ChannelAgentForEndAppService : CrudAppService<ChannelAgent, ChannelAgentDto>, IChannelAgentForEndAppService
    {
        public ChannelAgentForEndAppService(IRepository<ChannelAgent> respository) :base(respository)
        {

        }

        public ChannelAgentForCreateOrEditDto GetChannelAgentForCreateOrEdit(NullableIdDto input)
        {
            ChannelAgentForCreateOrEditDto channelAgent = new ChannelAgentForCreateOrEditDto();

            if (input.Id.HasValue)
            {
                channelAgent = Repository.Get(input.Id.Value).MapTo<ChannelAgentForCreateOrEditDto>();
            }
            return channelAgent;
        }


        public ChannelAgentForCreateOrEditDto CreateOrEdit(ChannelAgentForCreateOrEditDto input)
        {
            if (input.Id.HasValue)
            {
                CheckUpdatePermission();

                var entity = GetEntityById(input.Id.Value);
                ObjectMapper.Map(input, entity);
                CurrentUnitOfWork.SaveChanges();
                return entity.MapTo<ChannelAgentForCreateOrEditDto>();
            }
            else
            {
                CheckCreatePermission();
                var entity = input.MapTo<ChannelAgent>();

                Repository.Insert(entity);
                CurrentUnitOfWork.SaveChanges();
                return entity.MapTo<ChannelAgentForCreateOrEditDto>();
            }
        }
    }
}
