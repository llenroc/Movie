using Application.Authorization.End.Users.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System.Threading.Tasks;

namespace Application.Authorization.End.Users
{
    public interface IUserAppService : ICrudAppService<UserListDto,long, UserGetAllInput, CreateUserInput, UpdateUserInput>
    {
        UserListDto GetUserFromUserName(GetUserFromUserNameInput input);

        Task RefreshWeixinUserInfo(IdInput input);

        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<PagedResultDto<UserListDto>> GetUsers(UserGetAllInput input);

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(IdInput<long> input);

        Task ResetUserSpecificPermissions(IdInput<long> input);

        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(IdInput<long> input);
    }
}