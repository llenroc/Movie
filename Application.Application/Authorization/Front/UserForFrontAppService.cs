using Application.Authorization.Front.Dto;
using Application.Authorization.Users;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Application.Authorization.Front
{
    public class UserForFrontAppService:ApplicationDomainServiceBase, IUserForFrontAppService
    {
        public UserManager UserManager { get; set; }
        private IRepository<User, long> UserRepository;
        public UserForFrontAppService(IRepository<User, long> userRepository)
        {
            UserRepository = userRepository;
        }

        public CommonUserForProfileDto GetMyParent()
        {
            User user = UserRepository.Get(InfrastructureSession.UserId.Value);
            CommonUserForProfileDto parentUser = user.ParentUser.MapTo<CommonUserForProfileDto>();
            return parentUser;
        }

        public RankInfo GetRankInfo()
        {
            RankInfo RankInfo = new RankInfo()
            {
                MyRank = UserManager.GetRankOfUser(InfrastructureSession.UserId.Value),
                PageIndex = 1,
            };
            RankInfo.Items = UserRepository.GetAll().Where(model => model.IsHide == false&&model.IsSpreader==true).Take(100).OrderByDescending(model => model.Sales).MapTo<List<UserForRankDto>>();
            return RankInfo;
        }
    }
}
