﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    /// <summary>
    /// Defines scope of a setting.
    /// </summary>
    [Flags]
    public enum SettingScopes
    {
        /// <summary>
        /// Represents a setting that can be configured/changed for the application level.
        /// </summary>
        Application = 1,

        /// <summary>
        /// Represents a setting that can be configured/changed for each Tenant.
        /// This is reserved
        /// </summary>
        Tenant = 2,

        /// <summary>
        /// Represents a setting that can be configured/changed for each User.
        /// </summary>
        User = 4
    }
}