using Application.Wallets.Front.Dto;
using Application.Wallets.Entities;
using Infrastructure.Application.Services;
using Infrastructure.Collections.Extensions;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wallets.Front
{
    public class WalletRecordAppService : CrudAppService<
        WalletRecord,
        WalletRecordDto,
        int,
        WalletRecordGetAllInput
        >, IWalletRecordAppService
    {
        public WalletRecordAppService(IRepository<WalletRecord> repository) :base(repository)
        {

        }

        protected override IQueryable<WalletRecord> CreateFilteredQuery(WalletRecordGetAllInput input)
        {
            return Repository.GetAll().Where(model => model.UserId ==InfrastructureSession.UserId.Value);
        }
    }
}
