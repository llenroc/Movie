using Application.Spread.Front.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System.Threading.Tasks;

namespace Application.Spread.Front
{
    public interface ISpreadAppService:IApplicationService
    {
        Task<string> GetQrcode();
        PagedResultDto<ShareDto> GetShares(ShareGetAllInput input);
    }
}
