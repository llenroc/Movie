using Application.Wallets.End.Dto;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wallets.End
{
    public interface IWalletRecordForEndAppService : ICrudAppService<
        WalletRecordDto,
        int,
        WalletRecordGetAllInput>
    {
    }
}
