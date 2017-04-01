﻿using System.Web.Mvc;
using Infrastructure.Dependency;
using Castle.MicroKernel.Registration;

namespace Infrastructure.Web.Mvc.Controllers
{
    /// <summary>
    /// Registers all MVC Controllers derived from <see cref="Controller"/>.
    /// </summary>
    public class ControllerConventionalRegistrar : IConventionalDependencyRegistrar
    {
        /// <inheritdoc/>
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            context.IocManager.IocContainer.Register(Classes.FromAssembly(context.Assembly).BasedOn<Controller>().LifestyleTransient());
        }
    }
}
