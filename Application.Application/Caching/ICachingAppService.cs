using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Infrastructure.Application.DTO;
using Application.Caching.Dto;

namespace Application.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(string name);

        Task ClearAllCaches();
    }
}
