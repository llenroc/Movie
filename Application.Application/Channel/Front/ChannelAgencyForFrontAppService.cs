using Application.Authorization.Front.Dto;
using Application.Authorization.Users;
using Application.Channel.ChananlAgencys;
using Application.Channel.ChannelAgencies;
using Application.Channel.Front.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using Infrastructure.Collections.Extensions;
using Infrastructure.Domain.Repositories;
using System.Linq;
using Infrastructure.Linq.Extensions;

namespace Application.Channel.Front
{
    public class ChannelAgencyForFrontAppService : ApplicationAppServiceBase, IChannelAgencyForFrontAppService
    {
        protected IRepository<ChannelAgency> Respository;
        protected IRepository<User, long> UserRespository;
        public ChannelAgencyManager ChannelAgencyManager { get; set; }

        public ChannelAgencyForFrontAppService(IRepository<ChannelAgency> respository,
            IRepository<User, long> userRespository)
        {
            Respository = respository;
            UserRespository = userRespository;
        }

        public ChannelAgencyDto GetChannelAgency()
        {
            return Respository.GetAll().Where(model => model.UserId == InfrastructureSession.UserId.Value).FirstOrDefault().MapTo<ChannelAgencyDto>();
        }

        public MyChannelAgentInfo GetMyChannelAgentInfo()
        {
            MyChannelAgentInfo myChannelAgentInfo = new MyChannelAgentInfo()
            {
                ChannelAgency = ChannelAgencyManager.GetChannelAgencyOfUser(InfrastructureSession.UserId.Value).MapTo<ChannelAgencyDto>(),
                ApplyingChannelAgencyApply = ChannelAgencyManager.GetApplyingChannelAgencyApplyOfUser(InfrastructureSession.UserId.Value).MapTo<ChannelAgencyApplyDto>()
            };
            return myChannelAgentInfo;
        }


        public PagedResultDto<CommonUserForProfileDto> GetCustomersOfChannelAgency(CustomerGetAllInput input)
        {
            User user = GetCurrentUser();
            var query = UserRespository.GetAll().WhereIf(input.Depth == 1, model => model.ChannelAgencyId == user.UserChannelAgencyId)
                .WhereIf(input.ShouldBePotential, model => model.IsSpreader == false)
                .WhereIf(input.Depth == 2, model => model.ParentUserId.HasValue && model.ParentUser.ParentUserId == InfrastructureSession.UserId.Value)
                .WhereIf(input.Depth == 3, model => model.ParentUserId.HasValue && model.ParentUser.ParentUserId.HasValue && model.ParentUser.ParentUser.ParentUserId == InfrastructureSession.UserId.Value);
            var totalCount = query.Count();
            query = query.PageBy((input.PageIndex - 1) * input.PageSize, input.PageSize);

            var entities = query.ToList();

            return new PagedResultDto<CommonUserForProfileDto>(
                totalCount,
                input.PageIndex,
                input.PageSize,
                entities.Select(model=>ObjectMapper.MapTo<CommonUserForProfileDto>()).ToList()
            );
        }
    }
}
