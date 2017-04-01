using Infrastructure.Collections;
using Infrastructure.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration.Startup
{
    /// <summary>
    /// Used to configure multi-tenancy.
    /// </summary>
    internal class MultiTenancyConfig : IMultiTenancyConfig
    {
        /// <summary>
        /// Is multi-tenancy enabled?
        /// Default value: false.
        /// </summary>
        public bool IsEnabled { get; set; }


        public ITypeList<ITenantResolveContributer> Resolvers { get; }

        public MultiTenancyConfig()
        {
            Resolvers = new TypeList<ITenantResolveContributer>();
        }
    }
}
