using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.DTO;

namespace Application.Authorization.Front.Dto
{
    public class RankInfo:PagedResultDto<UserForRankDto>
    {
        public int MyRank { get; set; }
    }
}
