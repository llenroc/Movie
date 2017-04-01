using Application.IO;
using Application.Orders.Admins.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Admins.Exporting
{
    public interface IOrderListExcelExporter
    {
        FileDto ExportToFile(List<OrderForExportDto> orderListDtos);
    }
}
