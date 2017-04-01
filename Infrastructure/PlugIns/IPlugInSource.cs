using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Modules;

namespace Infrastructure.PlugIns
{
    public interface IPlugInSource
    {
        List<Type> GetModules();
    }

    public static class PlugInSourceExtensions
    {
        public static List<Type> GetModulesWithAllDependencies(this IPlugInSource plugInSource)
        {
            return plugInSource.GetModules().SelectMany(InfrastructureModule.FindDependedModuleTypesRecursivelyIncludingGivenModule).Distinct().ToList();
        }
    }
}
