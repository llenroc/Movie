﻿using Infrastructure.Collections;
using Infrastructure.Application.Navigation;

namespace Infrastructure.Configuration.Startup
{

    /// <summary>
    /// Used to configure navigation.
    /// </summary>
    public interface INavigationConfiguration
    {
        /// <summary>
        /// List of navigation providers.
        /// </summary>
        ITypeList<NavigationProvider> Providers { get; }
    }
}
