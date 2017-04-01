using Infrastructure.Modules;
using Infrastructure.Threading;
using Owin;
using System;

namespace Infrastructure.Owin
{
    /// <summary>
    /// OWIN extension methods for .
    /// </summary>
    public static class CommonFrameOwinExtensions
    {
        /// <summary>
        /// Uses .
        /// </summary>
        public static void UseCommonFrame(this IAppBuilder app)
        {
            ThreadCultureSanitizer.Sanitize();
        }

        /// <summary>
        /// Use this extension method if you don't initialize  in other way.
        /// Otherwise, use <see cref="UseCommonFrame"/>.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <typeparam name="TStartupModule">The type of the startup module.</typeparam>
        public static void UseCommonFrame<TStartupModule>(this IAppBuilder app) where TStartupModule : InfrastructureModule
        {
            app.UseCommonFrame<TStartupModule>(Bootstrapper => { });
        }

        /// <summary>
        /// Use this extension method if you don't initialize  in other way.
        /// Otherwise, use <see cref="UseCommonFrame"/>.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configureAction"></param>
        /// <typeparam name="TStartupModule">The type of the startup module.</typeparam>
        public static void UseCommonFrame<TStartupModule>(this IAppBuilder app, Action<Bootstrapper> configureAction)  where TStartupModule : InfrastructureModule
        {
            app.UseCommonFrame();
            var bootstrapper = app.Properties["_Bootstrapper.Instance"] as Bootstrapper;

            if (bootstrapper == null)
            {
                bootstrapper = Bootstrapper.Create<TStartupModule>();
                configureAction(bootstrapper);
                bootstrapper.Initialize();
            }
        }
    }
}
