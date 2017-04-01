using Application.Orders.Entities;
using Application.Wallets.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pay.PayProviders
{
    public interface IPayProvider
    {
        Task Refund(Order order, long refundFee);
    }
}
