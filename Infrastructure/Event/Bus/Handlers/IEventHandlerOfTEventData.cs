﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event.Bus.Handlers
{
    /// <summary>
    /// Defines an interface of a class that handles events of type <see cref="TEventData"/>.
    /// </summary>
    /// <typeparam name="TEventData">Event type to handle</typeparam>
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// </summary>
        /// <param name="eventData">Event data</param>
        void HandleEvent(TEventData eventData);
    }
}
