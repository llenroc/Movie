using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration.Tenant.Dto
{
    public class SpreadSettingsEditDto
    {
        public decimal UpgradeOrderMoney { get; set; }
        public bool MustBeSpreaderForSpread { get; set; }
    }
}
