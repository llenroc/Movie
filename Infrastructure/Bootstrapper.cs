using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Infrastructure.Configuration.Startup;
using Infrastructure.Dependency;
using Infrastructure.Dependency.Installers;
using Infrastructure.JetBrains.Annotations;
using Infrastructure.Modules;
using Infrastructure.PlugIns;
using System;

namespace Infrastructure
{
    /// <summary>
    /// This is the main class that is responsible to start entire  system.
    /// Prepares dependency injection and registers core components needed for startup.
    /// It must be instantiated and initialized (see <see cref="Initialize"/>) first in an application.
    /// </summary>
    public class Bootstrapper : IDisposable
    {
        /// <summary>
        /// Get the startup module of the application which depends on other used modules.
        /// </summary>
        public Type StartupModule { get; }

        /// <summary>
        /// A list of plug in folders.
        /// </summary>
        public PlugInSourceList PlugInSources { get; }

        /// <summary>
        /// Gets IIocManager object used by this class.
        /// </summary>
        public IIocManager IocManager { get; }

        /// <summary>
        /// Is this object disposed before?
        /// </summary>
        protected bool IsDisposed;
        private ModuleManager _moduleManager;
        private ILogger _logger;

        /// <summary>
        /// Creates a new <see cref="Bootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="InfrastructureModule"/>.</param>
        private Bootstrapper([NotNull] Type startupModule) : this(startupModule, Dependency.IocManager.Instance)
        {

        }

        /// <summary>
        /// Creates a new <see cref="Bootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="InfrastructureModule"/>.</param>
        /// <param name="iocManager">IIocManager that is used to bootstrap the  system</param>
        private Bootstrapper([NotNull] Type startupModule, [NotNull] IIocManager iocManager)
        {
            Check.NotNull(startupModule, nameof(startupModule));
            Check.NotNull(iocManager, nameof(iocManager));

            if (!typeof(InfrastructureModule).IsAssignableFrom(startupModule))
            {
                throw new ArgumentException($"{nameof(startupModule)} should be derived from {nameof(InfrastructureModule)}.");
            }
            StartupModule = startupModule;
            IocManager = iocManager;

            PlugInSources = new PlugInSourceList();
            _logger = NullLogger.Instance;
        }

        /// <summary>
        /// Creates a new <see cref="Bootstrapper"/> instance.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="InfrastructureModule"/>.</typeparam>
        public static Bootstrapper Create<TStartupModule>()where TStartupModule : InfrastructureModule
        {
            return new Bootstrapper(typeof(TStartupModule));
        }

        /// <summary>
        /// Creates a new <see cref="Bootstrapper"/> instance.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="InfrastructureModule"/>.</typeparam>
        /// <param name="iocManager">IIocManager that is used to bootstrap the  system</param>
        public static Bootstrapper Create<TStartupModule>([NotNull] IIocManager iocManager) where TStartupModule : InfrastructureModule
        {
            return new Bootstrapper(typeof(TStartupModule), iocManager);
        }

        /// <summary>
        /// Creates a new <see cref="Bootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="InfrastructureModule"/>.</param>
        public static Bootstrapper Create([NotNull] Type startupModule)
        {
            return new Bootstrapper(startupModule);
        }

        /// <summary>
        /// Creates a new <see cref="Bootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="InfrastructureModule"/>.</param>
        /// <param name="iocManager">IIocManager that is used to bootstrap the  system</param>
        public static Bootstrapper Create([NotNull] Type startupModule, [NotNull] IIocManager iocManager)
        {
            return new Bootstrapper(startupModule, iocManager);
        }

        /// <summary>
        /// Initializes the  system.
        /// </summary>
        public virtual void Initialize()
        {
            ResolveLogger();

            try
            {
                RegisterBootstrapper();
                IocManager.IocContainer.Install(new CoreInstaller());

                IocManager.Resolve<PlugInManager>().PlugInSources.AddRange(PlugInSources);
                IocManager.Resolve<StartupConfiguration>().Initialize();

                _moduleManager = IocManager.Resolve<ModuleManager>();
                _moduleManager.Initialize(StartupModule);
                _moduleManager.StartModules();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex.ToString(), ex);
                throw;
            }
        }

        private void ResolveLogger()
        {
            if (IocManager.IsRegistered<ILoggerFactory>())
            {
                _logger = IocManager.Resolve<ILoggerFactory>().Create(typeof(Bootstrapper));
            }
        }

        private void RegisterBootstrapper()
        {
            if (!IocManager.IsRegistered<Bootstrapper>())
            {
                IocManager.IocContainer.Register(Component.For<Bootstrapper>().Instance(this));
            }
        }

        /// <summary>
        /// Disposes the  system.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            _moduleManager?.ShutdownModules();
        }
    }
}
