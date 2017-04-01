using Application.Authorization.End.Users.Profile.Dto;
using Application.Configuration;
using Application.Security;
using Infrastructure.Authorization;
using Infrastructure.UI;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Application.Authorization.End.Users.Profile
{
    [InfrastructureAuthorize]
    public class ProfileAppService : ApplicationAppServiceBase, IProfileAppService
    {
        private readonly IAppFolders _appFolders;

        public ProfileAppService(
            IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            await CheckPasswordComplexity(input.NewPassword);

            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.ChangePasswordAsync(user.Id, input.CurrentPassword, input.NewPassword));
        }

        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var settingValue = await SettingManager.GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            var setting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(settingValue);

            return new GetPasswordComplexitySettingOutput
            {
                Setting = setting
            };
        }

        private async Task CheckPasswordComplexity(string password)
        {
            var passwordComplexitySettingValue = await SettingManager.GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            var passwordComplexitySetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(passwordComplexitySettingValue);
            var passwordComplexityChecker = new PasswordComplexityChecker();
            var passwordValid = passwordComplexityChecker.Check(passwordComplexitySetting, password);

            if (!passwordValid)
            {
                throw new UserFriendlyException(L("PasswordComplexityNotSatisfied"));
            }
        }
    }
}
