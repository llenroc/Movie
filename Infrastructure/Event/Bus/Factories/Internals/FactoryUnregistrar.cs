﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event.Bus.Factories.Internals
{
    /// <summary>
    /// Used to unregister a <see cref="IEventHandlerFactory"/> on <see cref="Dispose"/> method.
    /// </summary>
    internal class FactoryUnregistrar : IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly Type _eventType;
        private readonly IEventHandlerFactory _factory;

        public FactoryUnregistrar(IEventBus eventBus, Type eventType, IEventHandlerFactory factory)
        {
            _eventBus = eventBus;
            _eventType = eventType;
            _factory = factory;
        }

        public void Dispose()
        {
            _eventBus.Unregister(_eventType, _factory);
        }
    }
}
