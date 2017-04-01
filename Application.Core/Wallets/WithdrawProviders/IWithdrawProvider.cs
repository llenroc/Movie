using Application.Wallets.Entities;
using Infrastructure.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wallets.WithdrawProviders
{
    public interface IWithdrawProvider:IDomainService
    {
        Task Withdraw(WalletRecord walletRecord);
    }
}
