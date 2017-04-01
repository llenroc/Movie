using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Admins.BackgroundJobs
{
    public class BatchShipJobArgs
    {
        public string FilePath { get; set; }

        public BatchShipJobArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
