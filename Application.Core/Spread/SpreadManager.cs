using Application.Authorization.Users;
using Application.Configuration;
using Application.Wechats;
using Application.Wechats.TemplateMessages;
using Application.Wechats.TemplateMessages.TemplateMessageDatas;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.UI;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System.Threading.Tasks;
using Utility.Common;

namespace Application.Spread
{
    public class SpreadManager : ApplicationDomainServiceBase
    {
        public IRepository<User, long> UserRepository { get; set; }
        public TemplateMessageManager TemplateMessageManager { get; set; }

        public async Task CanSpreadAsync(UserIdentifier userIdentifier)
        {
            bool mustBeSpreaderForSpread = await SettingManager.GetSettingValueForTenantAsync<bool>(SpreadSettings.General.MustBeSpreaderForSpread, userIdentifier.TenantId.Value);

            if (mustBeSpreaderForSpread)
            {
                using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
                {
                    bool isSpread = UserRepository.Get(userIdentifier.UserId).IsSpreader;

                    if (!isSpread)
                    {
                        throw new UserFriendlyException(L("YouMustBeSpreadToGetSpreadPoster"));
                    }
                }
            }
        }

        public async Task CanSpreadAsync(User user)
        {
            bool mustBeSpreaderForSpread = await SettingManager.GetSettingValueForTenantAsync<bool>(SpreadSettings.General.MustBeSpreaderForSpread, user.TenantId.Value);

            if (mustBeSpreaderForSpread && !user.IsSpreader)
            {
                throw new UserFriendlyException(L("YouMustBeSpreadToGetSpreadPoster"));
            }
        }

        [UnitOfWork]
        public async Task SetAsSpreader(UserIdentifier userIdentifier)
        {
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId.Value))
            {
                User user = UserRepository.Get(userIdentifier.UserId);
                await SetAsSpreader(user);
            }
        }

        [UnitOfWork]
        public async Task SetAsSpreader(User user)
        {
            if (user.IsSpreader)
            {
                return;
            }
            user.IsSpreader = true;
            UserRepository.Update(user);

            WechatUserManager wechatUserManager = IocManager.Instance.Resolve<WechatUserManager>();
            string openid = wechatUserManager.GetOpenid(user.ToUserIdentifier());

            if (openid.HasValue())
            {
                try
                {
                    UpgradeTemplateMessageData data = new UpgradeTemplateMessageData(
                        new TemplateDataItem(L("CongratulationsOnBeenSpreader")),
                        new TemplateDataItem(user.Id.ToString()),
                        new TemplateDataItem(L("NoLimit")),
                        new TemplateDataItem(L("ThankYouForYourPatronage"))
                        );
                    await TemplateMessageManager.SendTemplateMessageOfUpgradeAsync(user.TenantId.Value, openid, null, data);
                }
                catch
                {

                }
            }
        }
    }
}
