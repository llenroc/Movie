using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event.Bus.Entities
{
    public enum EntityChangeType
    {
        Created,
        Updated,
        Deleted
    }
}
