using Application.Wallets.End.Dto;
using Application.Wallets.Entities;
using Infrastructure.Application.Services;
using Infrastructure.Collections.Extensions;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wallets.End
{
    public class WalletRecordForEndAppService : CrudAppService<
        WalletRecord,
        WalletRecordDto,
        int,
        WalletRecordGetAllInput
        >, IWalletRecordForEndAppService
    {
        public WalletRecordForEndAppService(IRepository<WalletRecord> repository) :base(repository)
        {

        }
    }
}
