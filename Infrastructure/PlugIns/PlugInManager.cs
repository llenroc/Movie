using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PlugIns
{
    public class PlugInManager : IPlugInManager
    {
        public PlugInSourceList PlugInSources { get; }

        public PlugInManager()
        {
            PlugInSources = new PlugInSourceList();
        }
    }
}
