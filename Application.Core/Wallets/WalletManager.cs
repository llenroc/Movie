using Application.Authorization.Users;
using Application.Entities;
using Application.Orders.Entities;
using Application.Orders.NumberProviders;
using Application.Wallets.Entities;
using Application.Wallets.WithdrawProviders;
using Application.Wechats;
using Application.Wechats.TemplateMessages;
using Application.Wechats.TemplateMessages.TemplateMessageDatas;
using Infrastructure;
using Infrastructure.Dependency;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Wallets
{
    public class WalletManager:ApplicationDomainServiceBase
    {
        public IRepository<Wallet> WalletRepository { get; set; }
        public IRepository<User,long> UserRepository { get; set; }
        public IRepository<WalletRecord> WalletRecordRepository { get; set; }
        public INumberProvider NumberProvider { get; set; }
        public TemplateMessageManager TemplateMessageManager { get; set; }
        public WechatUserManager WechatUserManager { get; set; }

        public WalletManager()
        {

        }

        [UnitOfWork]
        public Wallet GetWalletOfUser(UserIdentifier userIdentifier)
        {
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
            {
                Wallet wallet = WalletRepository.GetAll().Where(model => model.UserId == userIdentifier.UserId).FirstOrDefault();

                if (wallet == null)
                {
                    wallet = CreateWalletOfUser(userIdentifier);
                }
                return wallet;
            }
        }

        [UnitOfWork]
        private Wallet CreateWalletOfUser(UserIdentifier userIdentifier)
        {
            Wallet wallet = new Wallet()
            {
                UserId= userIdentifier.UserId
            };
            WalletRepository.Insert(wallet);
            unitOfWorkManager.Current.SaveChanges();
            return wallet;
        }

        [UnitOfWork]
        public async Task<WalletRecord> IncomeOfOrderRebate(UserIdentifier userIdentifier, decimal money, string remark,Order order)
        {
            WalletRecord walletRecord= BuildWalletRecord(WalletRecordType.Recharge, userIdentifier.UserId, money, remark); 
            WalletRecordRepository.Insert(walletRecord);
            Wallet wallet = GetWalletOfUser(userIdentifier);
            wallet.Money +=money;
            WalletRepository.Update(wallet);
            CurrentUnitOfWork.SaveChanges();

            string openid = WechatUserManager.GetOpenid(userIdentifier);

            if (!string.IsNullOrEmpty(openid))
            {
                OrderRebateTemplateMessageData data = new OrderRebateTemplateMessageData(
                    new TemplateDataItem(remark),
                    new TemplateDataItem(order.Number),
                    new TemplateDataItem(order.PayMoney.ToString()),
                    new TemplateDataItem(order.PaymentDatetime.ToString()),
                    new TemplateDataItem(money.ToString()),
                    new TemplateDataItem(L("ThankYouForYourPatronage"))
                    );
                await TemplateMessageManager.SendTemplateMessageOfOrderRebateAsync(order.TenantId, openid, null, data);
            }
            return walletRecord;
        }

        [UnitOfWork]
        public WalletRecord Recharge(UserIdentifier userIdentifier, decimal money,string remark)
        {
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
            {
                WalletRecord walletRecord = BuildWalletRecord(WalletRecordType.Recharge, userIdentifier.UserId, money, remark);
                WalletRecordRepository.Insert(walletRecord);

                Wallet wallet = GetWalletOfUser(userIdentifier);
                wallet.Money += money;
                WalletRepository.Update(wallet);

                CurrentUnitOfWork.SaveChanges();
                return walletRecord;
            }
        }

        [UnitOfWork]
        public async Task<WalletRecord> WithdrawAllBalanceAsync(UserIdentifier userIdentifier)
        {
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
            {
                Wallet wallet = GetWalletOfUser(userIdentifier);
                return await WithdrawAsync(userIdentifier, wallet.Money, L("Withdraw"));
            }
        }

        private WalletRecord BuildWalletRecord(WalletRecordType type, long userId, decimal money, string remark)
        {
            WalletRecord walletRecord = new WalletRecord(type, userId, money, remark);
            walletRecord.SerialNumber = NumberProvider.BuildNumber();
            return walletRecord;
        }

        [UnitOfWork]
        public async Task<WalletRecord> WithdrawAsync(UserIdentifier userIdentifier, decimal money, string remark)
        {
            using (CurrentUnitOfWork.DisableFilter(DataFilters.MustHaveTenant))
            {
                WalletRecord walletRecord = BuildWalletRecord(WalletRecordType.Withdraw, userIdentifier.UserId, -money, remark);
                WalletRecordRepository.Insert(walletRecord);

                Wallet wallet = GetWalletOfUser(userIdentifier);
                wallet.Money -= money;
                WalletRepository.Update(wallet);

                CurrentUnitOfWork.SaveChanges();

                await ProcessWithdrawAsync(walletRecord);
                return walletRecord;
            }
        }

        public async Task<WalletRecord> ProcessWithdrawAsync(WalletRecord walletRecord)
        {
            IWithdrawProvider WithdrawProvider = IocManager.Instance.Resolve<IWithdrawProvider>();

            try
            {
                await WithdrawProvider.Withdraw(walletRecord);
            }
            catch (Exception exception)
            {
                WithdrawNotify(walletRecord, false, exception.Message);
            }
            return walletRecord;
        }

        [UnitOfWork]
        public WalletRecord WithdrawNotify(WalletRecord walletRecord, bool success, string failReason =null)
        {
            using (CurrentUnitOfWork.SetTenantId(walletRecord.TenantId))
            {
                string openid = WechatUserManager.GetOpenid(walletRecord.GetUserIdentifier());
                User user = walletRecord.User;

                if (user == null)
                {
                    user = UserRepository.Get(walletRecord.UserId);
                }

                if (success)
                {
                    walletRecord.FetchStatus = FetchStatus.Success;
                    walletRecord.FailReason = "";
                    WalletRecordRepository.Update(walletRecord);
                    CurrentUnitOfWork.SaveChanges();

                    if (!string.IsNullOrEmpty(openid))
                    {
                        Task.Run(async () =>
                        {
                            WalletWithdrawTemplateMessageData data = new WalletWithdrawTemplateMessageData(
                                  new TemplateDataItem(L("WithdrawSuccessfully")),
                                  new TemplateDataItem(user.NickName),
                                  new TemplateDataItem((-walletRecord.Money).ToString()),
                                  new TemplateDataItem(L("ThankYouForYourPatronage"))
                                  );
                            await TemplateMessageManager.SendTemplateMessageOfWalletWithdrawAsync(walletRecord.TenantId, openid, null, data);
                        });
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(failReason))
                    {
                        failReason = L("UnKnowFail");
                    }
                    walletRecord.FetchStatus = FetchStatus.Fail;
                    walletRecord.FailReason = failReason;
                    WalletRecordRepository.Update(walletRecord);
                    CurrentUnitOfWork.SaveChanges();

                    if (!string.IsNullOrEmpty(openid))
                    {
                        Task.Run(async () =>
                        {
                            WalletWithdrawTemplateMessageData data = new WalletWithdrawTemplateMessageData(
                                  new TemplateDataItem(L("WithdrawFailed")+ ":" + failReason),
                                  new TemplateDataItem(user.NickName),
                                  new TemplateDataItem((-walletRecord.Money).ToString()),
                                  new TemplateDataItem(L("ThankYouForYourPatronage"))
                                  );
                            await TemplateMessageManager.SendTemplateMessageOfWalletWithdrawAsync(walletRecord.TenantId, openid, null, data);
                        });
                    }
                }
                return walletRecord;
            }
        }

        [UnitOfWork]
        public WalletRecord Transaction(UserIdentifier userIdentifier, decimal money, string remark)
        {
            WalletRecord walletRecord = BuildWalletRecord(WalletRecordType.Transaction, userIdentifier.UserId, -money, remark);
            WalletRecordRepository.Insert(walletRecord);
            CurrentUnitOfWork.SaveChanges();
            return walletRecord;
        }

        public decimal GetMoneyOfRecharge(long userId)
        {
            return WalletRecordRepository.GetAll().Where(model => model.Type == WalletRecordType.Recharge
            &&model.UserId==userId).Sum(od => ((decimal?)od.Money)).GetValueOrDefault();
        }

        public decimal GetMoneyOfWithdrawed(long userId)
        {
            return WalletRecordRepository.GetAll().Where(model => model.Type == WalletRecordType.Withdraw 
            && model.FetchStatus == FetchStatus.Success
            &&model.UserId==userId
            ).Sum(od => ((decimal?)od.Money)).GetValueOrDefault();
        }

        public decimal GetMoneyOfWithdrawing(long userId)
        {
            return WalletRecordRepository.GetAll().Where(model => model.Type == WalletRecordType.Withdraw
            &&model.FetchStatus!=FetchStatus.Success
            && model.UserId == userId).Sum(od => ((decimal?)od.Money)).GetValueOrDefault();
        }

        [UnitOfWork]
        public WalletRecord SetPayStatusOfSuccess(WalletRecord walletRecord)
        {
            walletRecord.PayStatus = PayStatus.Success;
            walletRecord.PayDateTime = DateTime.Now;

            Wallet wallet = GetWalletOfUser(walletRecord.GetUserIdentifier());

            if (walletRecord.Type == WalletRecordType.Recharge)
            {
                wallet.Money += walletRecord.Money;
            }
            WalletRepository.Update(wallet);
            WalletRecordRepository.Update(walletRecord);
            CurrentUnitOfWork.SaveChanges();
            return walletRecord;
        }

        [UnitOfWork]
        public WalletRecord SetPayStatusOfFail(WalletRecord walletRecord,string failReason=null)
        {
            walletRecord.PayStatus = PayStatus.Fail;
            walletRecord.FailReason = failReason;
            WalletRecordRepository.Update(walletRecord);
            CurrentUnitOfWork.SaveChanges();
            return walletRecord;
        }
    }
}