using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MultiTenancy
{
    public class TenantInfo
    {
        public int Id { get; set; }

        public string TenancyName { get; set; }

        public TenantInfo(int id, string tenancyName)
        {
            Id = id;
            TenancyName = tenancyName;
        }
    }
}
