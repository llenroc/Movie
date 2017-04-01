﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    /// <summary>
    /// Can be added to a method to enable auto validation if validation is disabled for it's class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EnableValidationAttribute : Attribute
    {

    }
}
