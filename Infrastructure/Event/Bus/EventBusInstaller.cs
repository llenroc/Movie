﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.Dependency;
using Infrastructure.Event.Bus.Handlers;
using Infrastructure.Event.Bus.Factories;
using Infrastructure.Configuration.Startup;

namespace Infrastructure.Event.Bus
{
    /// <summary>
    /// Installs event bus system and registers all handlers automatically.
    /// </summary>
    internal class EventBusInstaller : IWindsorInstaller
    {
        private readonly IIocResolver _iocResolver;
        private readonly IEventBusConfiguration _eventBusConfiguration;
        private IEventBus _eventBus;

        public EventBusInstaller(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            _eventBusConfiguration = iocResolver.Resolve<IEventBusConfiguration>();
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (_eventBusConfiguration.UseDefaultEventBus)
            {
                container.Register(Component.For<IEventBus>().UsingFactoryMethod(() => EventBus.Default).LifestyleSingleton());
            }
            else
            {
                container.Register(Component.For<IEventBus>().ImplementedBy<EventBus>().LifestyleSingleton());
            }
            _eventBus = container.Resolve<IEventBus>();
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            /* This code checks if registering component implements any IEventHandler<TEventData> interface, if yes,
             * gets all event handler interfaces and registers type to Event Bus for each handling event.
             */
            if (!typeof(IEventHandler).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }
            var interfaces = handler.ComponentModel.Implementation.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).IsAssignableFrom(@interface))
                {
                    continue;
                }
                var genericArgs = @interface.GetGenericArguments();

                if (genericArgs.Length == 1)
                {
                    _eventBus.Register(genericArgs[0], new IocHandlerFactory(_iocResolver, handler.ComponentModel.Implementation));
                }
            }
        }
    }
}
