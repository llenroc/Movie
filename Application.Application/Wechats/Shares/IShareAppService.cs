using Application.Wechats.Shares.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wechats.Shares
{
    public interface IShareAppService : ICrudAppService<ShareDto, int, PagedAndSortedResultRequestDto, ShareForCreateInput, ShareDto>
    {
        PreShareOutput GetPreShare();
        ShareDto Share(ShareForCreateInput shareForCreateInput);
    }
}
