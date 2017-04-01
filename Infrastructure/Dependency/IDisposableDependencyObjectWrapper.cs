﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    /// <summary>
    /// This interface is used to wrap an object that is resolved from IOC container.
    /// It inherits <see cref="IDisposable"/>, so resolved object can be easily released.
    /// In <see cref="IDisposable.Dispose"/> method, <see cref="IIocResolver.Release"/> is called to dispose the object.
    /// This is non-generic version of <see cref="IDisposableDependencyObjectWrapper{T}"/> interface.
    /// </summary>
    public interface IDisposableDependencyObjectWrapper : IDisposableDependencyObjectWrapper<object>
    {

    }
}