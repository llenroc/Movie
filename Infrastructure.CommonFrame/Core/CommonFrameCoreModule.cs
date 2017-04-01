using System.Linq;
using System.Reflection;
using Infrastructure.Application.Features;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.Dependency;
using Infrastructure.Localization;
using Infrastructure.Localization.Dictionaries;
using Infrastructure.Localization.Dictionaries.Xml;
using Infrastructure.Modules;
using Infrastructure.MultiTenancy;
using Infrastructure.Reflection;
using Infrastructure.CommonFrame.Configuration;
using Castle.MicroKernel.Registration;
using Infrastructure.CommonFrame.MultiTenancy;
using Infrastructure.Configuration.Startup;

namespace Infrastructure.CommonFrame
{
    /// <summary>
    ///  CommonFrame core module.
    /// </summary>
    [DependsOn(typeof(KernelModule))]
    public class CommonFrameCoreModule : InfrastructureModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            IocManager.Register<ILanguageManagementConfig, LanguageManagementConfig>();
            IocManager.Register<ICommonFrameEntityTypes, CommonFrameEntityTypes>();
            IocManager.Register<ICommonFrameConfig, CommonFrameConfig>();

            Configuration.ReplaceService<ITenantStore, TenantStore>(DependencyLifeStyle.Transient);

            Configuration.Settings.Providers.Add<CommonFrameSettingProvider>();

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    CommonFrameConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(Assembly.GetExecutingAssembly(),
                    "Infrastructure.CommonFrame.Core.Localization.Source")));

            IocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        public override void Initialize()
        {
            FillMissingEntityTypes();

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register<IMultiTenantLocalizationDictionary, MultiTenantLocalizationDictionary>(DependencyLifeStyle.Transient); //could not register conventionally

            RegisterTenantCache();
        }

        private void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            if (typeof(ICommonFrameFeatureValueStore).IsAssignableFrom(handler.ComponentModel.Implementation) && !IocManager.IsRegistered<ICommonFrameFeatureValueStore>())
            {
                IocManager.IocContainer.Register(
                    Component.For<ICommonFrameFeatureValueStore>().ImplementedBy(handler.ComponentModel.Implementation).Named("ZeroFeatureValueStore").LifestyleTransient()
                    );
            }
        }

        private void FillMissingEntityTypes()
        {
            using (var entityTypes = IocManager.ResolveAsDisposable<ICommonFrameEntityTypes>())
            {
                if (entityTypes.Object.User != null && entityTypes.Object.Role != null && entityTypes.Object.Tenant != null)
                {
                    return;
                }

                using (var typeFinder = IocManager.ResolveAsDisposable<ITypeFinder>())
                {
                    var types = typeFinder.Object.FindAll();
                    entityTypes.Object.Tenant = types.FirstOrDefault(t => typeof(TenantBase).IsAssignableFrom(t) && !t.IsAbstract);
                    entityTypes.Object.Role = types.FirstOrDefault(t => typeof(RoleBase).IsAssignableFrom(t) && !t.IsAbstract);
                    entityTypes.Object.User = types.FirstOrDefault(t => typeof(UserBase).IsAssignableFrom(t) && !t.IsAbstract);
                }
            }
        }

        private void RegisterTenantCache()
        {
            if (IocManager.IsRegistered<ITenantCache>())
            {
                return;
            }

            using (var entityTypes = IocManager.ResolveAsDisposable<ICommonFrameEntityTypes>())
            {
                var implType = typeof(TenantCache<,>).MakeGenericType(entityTypes.Object.Tenant, entityTypes.Object.User);

                IocManager.Register(typeof(ITenantCache), implType, DependencyLifeStyle.Transient);
            }
        }
    }
}
