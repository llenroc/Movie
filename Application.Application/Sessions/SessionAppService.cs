using Application.Sessions.Dto;
using Application.Shops;
using Infrastructure.Auditing;
using Infrastructure.AutoMapper;
using System.Threading.Tasks;

namespace Application.Sessions
{
    public class SessionAppService : ApplicationAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<CurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync()
        {
            var output = new CurrentLoginInformationsOutput
            {
                User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>()
            };

            if (InfrastructureSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }
            return output;
        }

        [DisableAuditing]
        public CurrentLoginInformationsOutput GetCurrentLoginInformations()
        {
            var output = new CurrentLoginInformationsOutput
            {
                User = GetCurrentUser().MapTo<UserLoginInfoDto>()
            };

            if (InfrastructureSession.TenantId.HasValue)
            {
                output.Tenant = GetCurrentTenant().MapTo<TenantLoginInfoDto>();
            }
            return output;
        }

        public async Task<ShopInformationsOutput> GetShopInformations()
        {
            var output = new ShopInformationsOutput
            {
                Name= await SettingManager.GetSettingValueForTenantAsync(ShopSettings.General.Name, InfrastructureSession.TenantId.Value),
                Logo = await SettingManager.GetSettingValueForTenantAsync(ShopSettings.General.Logo, InfrastructureSession.TenantId.Value),

                ShareTitle = await SettingManager.GetSettingValueForTenantAsync(ShopSettings.Share.ShareTitle, InfrastructureSession.TenantId.Value),
                ShareDescription = await SettingManager.GetSettingValueForTenantAsync(ShopSettings.Share.ShareDescription, InfrastructureSession.TenantId.Value),
                SharePicture = await SettingManager.GetSettingValueForTenantAsync(ShopSettings.Share.SharePicture, InfrastructureSession.TenantId.Value),
            };
            return output;
        }
    }
}