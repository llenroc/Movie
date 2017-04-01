using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Infrastructure.Domain.Repositories;
using Infrastructure.Application.DTO;
using Application.Wechats.Shares.Dto;

namespace Application.Wechats.Shares
{
    public class ShareAppService : CrudAppService<Share, ShareDto,int,PagedAndSortedResultRequestDto,ShareForCreateInput,ShareDto>, IShareAppService
    {
        public ShareAppService(IRepository<Share> respository) :base(respository)
        {

        }

        public PreShareOutput GetPreShare()
        {
            PreShareOutput preShareOutput = new PreShareOutput();
            preShareOutput.No = Shares.Share.CreateNo();
            return preShareOutput;
        }

        public ShareDto Share(ShareForCreateInput shareForCreateInput)
        {
            ShareDto share = Create(shareForCreateInput);
            return share;
        }
    }
}
