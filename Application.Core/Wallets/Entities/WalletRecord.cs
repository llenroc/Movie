using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using Application.Entities;
using Application.Authorization.Users;

namespace Application.Wallets.Entities
{
    public enum PayType
    {
        Undetermined,
        Alipay,
        WeChat,
        BankCard,
        Balance
    }

    public enum PayStatus
    {
        UnPay,
        Success,
        Fail
    }

    public enum WalletRecordType
    {
        Recharge,
        InCome,
        Withdraw,
        Transaction
    }

    public enum FetchStatus
    {
        UnFetch,
        Success,
        Fail
    }

    public class WalletRecord : FullAuditedEntity, IUserIdentifierEntity
    {
        public int TenantId { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public long? FromUserId { get; set; }

        public long? ToUserId { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }

        public WalletRecordType Type { get; set; }

        public PayType? PayType { get; set; }

        public decimal Money { get; set; }

        public string Remark { get; set; }

        public PayStatus PayStatus { get; set; } = PayStatus.UnPay;

        public DateTime? PayDateTime { get; set; }

        public FetchStatus FetchStatus { get; set; } = FetchStatus.UnFetch;

        public DateTime? FetchDateTime { get; set; }

        public string FailReason { get; set; }

        public string GetTypeText()
        {
            return Type.ToString();
        }

        public string GetPayStatusText()
        {
            return PayStatus.ToString();
        }

        public string GetFetchStatusText()
        {
            return FetchStatus.ToString();
        }

        public WalletRecord()
        {

        }

        public WalletRecord(WalletRecordType type,long userId, decimal money,string remark)
        {
            Type = type;
            UserId = userId;
            Money = money;
            Remark = remark;
        }
    }
}
