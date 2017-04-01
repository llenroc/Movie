using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.PlugIns;
using Infrastructure.Logging;
using System.Web.Compilation;

namespace Infrastructure.Web.Web
{
    public static class PlugInSourceListExtensions
    {
        public static void AddToBuildManager(this PlugInSourceList plugInSourceList)
        {
            var plugInAssemblies = plugInSourceList
                .GetAllModules()
                .Select(m => m.Assembly)
                .Distinct()
                .ToList();

            foreach (var plugInAssembly in plugInAssemblies)
            {
                try
                {
                    LogHelper.Logger.Debug($"Adding {plugInAssembly.FullName} to BuildManager");
                    BuildManager.AddReferencedAssembly(plugInAssembly);
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Warn(ex.ToString(), ex);
                }
            }
        }
    }
}
