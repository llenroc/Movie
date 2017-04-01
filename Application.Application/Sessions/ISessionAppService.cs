using Application.Sessions.Dto;
using Infrastructure.Application.Services;
using System.Threading.Tasks;

namespace Application.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<CurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();

        CurrentLoginInformationsOutput GetCurrentLoginInformations();

        Task<ShopInformationsOutput> GetShopInformations();
    }
}
