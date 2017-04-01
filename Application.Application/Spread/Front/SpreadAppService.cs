using Application.Spread.Front.Dto;
using Application.Spread.SpreadPosters;
using Application.Wechats.Qrcodes;
using Application.Wechats.Shares;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using Infrastructure.Linq.Extensions;
using Infrastructure.Runtime.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Spread.Front
{
    public class SpreadAppService:ApplicationAppServiceBase, ISpreadAppService
    {
        public SpreadPosterManager SpreadPosterManager { get; set; }
        public QrcodeManager QrcodeManager { get; set; }
        private IRepository<Share> ShareRepository;

        public SpreadAppService(IRepository<Share> shareRepository)
        {
            ShareRepository = shareRepository;
        }

        public async Task<string> GetQrcode()
        {
            Qrcode qrcode = await QrcodeManager.GetQrcodeAsync(InfrastructureSession.ToUserIdentifier());
            return qrcode.Path;
        }

        public PagedResultDto<ShareDto> GetShares(ShareGetAllInput input)
        {
            var query = ShareRepository.GetAll();
            var totalCount = query.Count();
            query = query.OrderByDescending(model=>model.CreationTime).PageBy(input);
            var entities = query.ToList();
            return new PagedResultDto<ShareDto>(
                totalCount,
                input.PageIndex,
                input.PageSize,
                entities.MapTo<List<ShareDto>>()
            );
        }
    }
}
