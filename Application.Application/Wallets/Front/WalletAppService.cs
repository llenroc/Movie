using Application.Wallets.Front.Dto;
using Application.Wallets;
using Application.Wallets.Entities;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Runtime.Session;

namespace Application.Wallets.Front
{
    public class WalletAppService: ApplicationAppServiceBase, IWalletAppService
    {
        private readonly IRepository<Wallet> _walletRepository;
        public WalletManager WalletManager { get; set; }

        public WalletAppService(IRepository<Wallet> walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task Withdraw()
        {
            await WalletManager.WithdrawAllBalanceAsync(InfrastructureSession.ToUserIdentifier());
        }

        public WalletInfoOutput GetWalletInfo()
        {
            Wallet wallet = WalletManager.GetWalletOfUser(InfrastructureSession.ToUserIdentifier());
            WalletInfoOutput walletInfoOutput = new WalletInfoOutput()
            {
                Balance = wallet.Money,
                Withdrawed=WalletManager.GetMoneyOfWithdrawed(InfrastructureSession.UserId.Value),
                Withdrawing= WalletManager.GetMoneyOfWithdrawing(InfrastructureSession.UserId.Value),
                TotalIncome=WalletManager.GetMoneyOfRecharge(InfrastructureSession.UserId.Value)
            };
            return walletInfoOutput;
        }
    }
}
