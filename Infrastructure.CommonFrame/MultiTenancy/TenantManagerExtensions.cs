using System.Collections.Generic;
using Infrastructure.Authorization.Users;
using Infrastructure.Threading;
using Microsoft.AspNet.Identity;

namespace Infrastructure.MultiTenancy
{
    public static class TenantManagerExtensions
    {
        public static IdentityResult Create<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.CreateAsync(tenant));
        }

        public static IdentityResult Update<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.UpdateAsync(tenant));
        }

        public static TTenant FindById<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int id)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.FindByIdAsync(id));
        }

        public static TTenant GetById<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int id)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetByIdAsync(id));
        }

        public static TTenant FindByTenancyName<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, string tenancyName)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.FindByTenancyNameAsync(tenancyName));
        }

        public static IdentityResult Delete<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.DeleteAsync(tenant));
        }

        public static string GetFeatureValueOrNull<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int tenantId, string featureName)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetFeatureValueOrNullAsync(tenantId, featureName));
        }

        public static IReadOnlyList<NameValue> GetFeatureValues<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int tenantId)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetFeatureValuesAsync(tenantId));
        }

        public static void SetFeatureValues<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int tenantId, params NameValue[] values)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValuesAsync(tenantId, values));
        }

        public static void SetFeatureValue<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int tenantId, string featureName, string value)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValueAsync(tenantId, featureName, value));
        }

        public static void SetFeatureValue<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, TTenant tenant, string featureName, string value)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValueAsync(tenant, featureName, value));
        }

        public static void ResetAllFeatures<TTenant, TUser>(this TenantManager<TTenant, TUser> tenantManager, int tenantId)
            where TTenant : CommonFrameTenant<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.ResetAllFeaturesAsync(tenantId));
        }

    }
}
