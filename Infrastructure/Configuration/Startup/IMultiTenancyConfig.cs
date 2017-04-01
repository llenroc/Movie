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
    public interface IMultiTenancyConfig
    {
        /// <summary>
        /// Is multi-tenancy enabled?
        /// Default value: false.
        /// </summary>
        bool IsEnabled { get; set; }


        /// <summary>
        /// A list of contributers for tenant resolve process.
        /// </summary>
        ITypeList<ITenantResolveContributer> Resolvers { get; }
    }
}
