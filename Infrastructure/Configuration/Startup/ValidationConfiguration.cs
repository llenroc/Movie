﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Configuration.Startup
{

    public class ValidationConfiguration : IValidationConfiguration
    {
        public List<Type> IgnoredTypes { get; }

        public ValidationConfiguration()
        {
            IgnoredTypes = new List<Type>();
        }
    }
}
