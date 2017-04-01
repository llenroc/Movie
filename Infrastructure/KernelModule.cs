﻿using Castle.MicroKernel.Registration;
using Infrastructure.Application.Features;
using Infrastructure.Application.Navigation;
using Infrastructure.Application.Services;
using Infrastructure.Auditing;
using Infrastructure.Authorization;
using Infrastructure.BackgroundJobs;
using Infrastructure.Collections.Extensions;
using Infrastructure.Configuration.Startup;
using Infrastructure.Dependency;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Event.Bus;
using Infrastructure.Localization;
using Infrastructure.Localization.Dictionaries;
using Infrastructure.Localization.Dictionaries.Xml;
using Infrastructure.Modules;
using Infrastructure.Net.Mail;
using Infrastructure.Notifications;
using Infrastructure.Runtime.Caching;
using Infrastructure.Runtime.Validation.Interception;
using Infrastructure.Threading;
using Infrastructure.Threading.BackgroundWorkers;
using Infrastructure.Timing;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>
    /// Kernel (core) module of the  system.
    /// No need to depend on this, it's automatically the first module always.
    /// </summary>
    public sealed class KernelModule : InfrastructureModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            ValidationInterceptorRegistrar.Initialize(IocManager);
            AuditingInterceptorRegistrar.Initialize(IocManager);
            UnitOfWorkRegistrar.Initialize(IocManager);
            AuthorizationInterceptorRegistrar.Initialize(IocManager);

            Configuration.Auditing.Selectors.Add(new NamedTypeSelector("Infrastructure.ApplicationServices",type => typeof(IApplicationService).IsAssignableFrom(type)));
  
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    Consts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Infrastructure.Localization.Sources.XmlSource"
                    )));

            Configuration.Settings.Providers.Add<LocalizationSettingProvider>();
            Configuration.Settings.Providers.Add<EmailSettingProvider>();
            Configuration.Settings.Providers.Add<NotificationSettingProvider>();
            Configuration.Settings.Providers.Add<TimingSettingProvider>();

            Configuration.UnitOfWork.RegisterFilter(DataFilters.SoftDelete, true);
            Configuration.UnitOfWork.RegisterFilter(DataFilters.MustHaveTenant, true);
            Configuration.UnitOfWork.RegisterFilter(DataFilters.MayHaveTenant, true);

            ConfigureCaches();
            AddIgnoredTypes();
        }

        public override void Initialize()
        {
            foreach (var replaceAction in ((StartupConfiguration)Configuration).ServiceReplaceActions.Values)
            {
                replaceAction();
            }
            IocManager.IocContainer.Install(new EventBusInstaller(IocManager));

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(),
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
        }

        public override void PostInitialize()
        {
            RegisterMissingComponents();

            IocManager.Resolve<SettingDefinitionManager>().Initialize();
            IocManager.Resolve<FeatureManager>().Initialize();
            IocManager.Resolve<PermissionManager>().Initialize();
            IocManager.Resolve<LocalizationManager>().Initialize();
            IocManager.Resolve<NotificationDefinitionManager>().Initialize();
            IocManager.Resolve<NavigationManager>().Initialize();

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workerManager.Start();
                workerManager.Add(IocManager.Resolve<IBackgroundJobManager>());
            }
        }

        public override void Shutdown()
        {
            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.Resolve<IBackgroundWorkerManager>().StopAndWaitToStop();
            }
        }

        private void ConfigureCaches()
        {
            Configuration.Caching.Configure(CacheNames.ApplicationSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(8);
            });

            Configuration.Caching.Configure(CacheNames.TenantSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(60);
            });

            Configuration.Caching.Configure(CacheNames.UserSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(20);
            });
        }

        private void AddIgnoredTypes()
        {
            var commonIgnoredTypes = new[] { typeof(Stream), typeof(Expression) };

            foreach (var ignoredType in commonIgnoredTypes)
            {
                Configuration.Auditing.IgnoredTypes.AddIfNotContains(ignoredType);
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }

            var validationIgnoredTypes = new[] { typeof(Type) };
            foreach (var ignoredType in validationIgnoredTypes)
            {
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }

        private void RegisterMissingComponents()
        {
            if (!IocManager.IsRegistered<IGuidGenerator>())
            {
                IocManager.IocContainer.Register(Component.For<IGuidGenerator, SequentialGuidGenerator>().Instance(SequentialGuidGenerator.Instance)
                );
            }
            IocManager.RegisterIfNot<IUnitOfWork, NullUnitOfWork>(DependencyLifeStyle.Transient);
            IocManager.RegisterIfNot<IAuditingStore, SimpleLogAuditingStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IPermissionChecker, NullPermissionChecker>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IRealTimeNotifier, NullRealTimeNotifier>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<INotificationStore, NullNotificationStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IUnitOfWorkFilterExecuter, NullUnitOfWorkFilterExecuter>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IClientInfoProvider, NullClientInfoProvider>(DependencyLifeStyle.Singleton);

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.RegisterIfNot<IBackgroundJobStore, InMemoryBackgroundJobStore>(DependencyLifeStyle.Singleton);
            }
            else
            {
                IocManager.RegisterIfNot<IBackgroundJobStore, NullBackgroundJobStore>(DependencyLifeStyle.Singleton);
            }
        }
    }
}
