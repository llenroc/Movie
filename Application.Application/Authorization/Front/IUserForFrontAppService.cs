using Application.Authorization.Front.Dto;
using Application.Group.Dto;
using Infrastructure.Application.Services;

namespace Application.Authorization.Front
{
    public interface IUserForFrontAppService:IApplicationService
    {
        RankInfo GetRankInfo();

        CommonUserForProfileDto GetMyParent();
    }
}