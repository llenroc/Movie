using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration.Tenant.Dto
{
    public class TenantSettingsEditDto
    {
        public ShopSettingsEditDto Shop { get; set; }

        public SpreadSettingsEditDto Spread { get; set; }
    }
}
