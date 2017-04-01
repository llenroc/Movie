using Application.Wallets.Front.Dto;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wallets.Front
{
    public interface IWalletAppService: IApplicationService
    {
        WalletInfoOutput GetWalletInfo();
        Task Withdraw();
    }
}
