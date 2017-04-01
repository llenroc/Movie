using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auditing
{
    public class NullClientInfoProvider : IClientInfoProvider
    {
        public static NullClientInfoProvider Instance { get; } = new NullClientInfoProvider();

        public string BrowserInfo => null;
        public string ClientIpAddress => null;
        public string ComputerName => null;
    }
}
