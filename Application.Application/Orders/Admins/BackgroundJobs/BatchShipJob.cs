using Application.IO;
using Application.Orders.Admins.Dto;
using Infrastructure.BackgroundJobs;
using Infrastructure.Dependency;
using Infrastructure.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Orders.Admins.BackgroundJobs
{
    public class BatchShipJob : BackgroundJob<BatchShipJobArgs>, ITransientDependency
    {
        public ExcelHelper ExcelHelper { get; set; }
        public CommonOrderManager OrderManager { get; set; }

        public override void Execute(BatchShipJobArgs batchShipJobArgs)
        {
            AsyncHelper.RunSync(async() =>
            {
                List<OrderOfBatchShipInput> orderOfBatchShipInputs = ExcelHelper.LoadFromExcel<OrderOfBatchShipInput>(batchShipJobArgs.FilePath).ToList();

                foreach (OrderOfBatchShipInput orderOfBatchShipInput in orderOfBatchShipInputs)
                {
                    await OrderManager.Ship(orderOfBatchShipInput.Number, orderOfBatchShipInput.ExpressCompany, orderOfBatchShipInput.TrackingNumber);
                }
            });
        }
    }
}