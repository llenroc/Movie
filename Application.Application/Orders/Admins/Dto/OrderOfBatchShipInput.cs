using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Admins.Dto
{
    public class OrderOfBatchShipInput
    {
        public string Number { get; set; }

        public string ExpressCompany { get; set; }

        public string TrackingNumber { get; set; }
    }
}
