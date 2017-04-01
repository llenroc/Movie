using Application.Wallets;
using Application.Wallets.Entities;
using Infrastructure.Dependency;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Threading;
using Infrastructure.Threading.BackgroundWorkers;
using Infrastructure.Threading.Timers;
using System;

namespace Application.BackgroundWorker
{
    public class WithdrawFailedWithdrawBackgroundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<WalletRecord> _walletRecordRepository;
        public WalletManager WalletManager { get; set; }

        public WithdrawFailedWithdrawBackgroundWorker(InfrastructureTimer timer, IRepository<WalletRecord> walletRecordRepository) 
            : base(timer)
        {
            _walletRecordRepository = walletRecordRepository;
            Timer.Period = 7200000;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(DataFilters.MustHaveTenant))
            {
                DateTime dateTime = DateTime.Now.AddHours(-2);

                AsyncHelper.RunSync(async() =>
                {
                    var walletRecords = _walletRecordRepository.GetAllList(model => model.FetchStatus == FetchStatus.Fail
                    && model.CreationTime < dateTime);

                    foreach (var walletRecord in walletRecords)
                    {
                        await WalletManager.ProcessWithdrawAsync(walletRecord);
                    }
                });
            }
        }
    }
}
