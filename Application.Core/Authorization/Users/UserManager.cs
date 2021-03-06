﻿using Infrastructure.Authorization;
using Infrastructure.Authorization.Users;
using Infrastructure.Configuration;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.IdentityFramework;
using Infrastructure.Localization;
using Infrastructure.Organizations;
using Infrastructure.Runtime.Caching;
using Application.Authorization.Roles;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Threading;
using System;
using Application.Channel.ChananlAgencys;
using System.Linq;
using Infrastructure.Event.Bus;
using Application.Authorization.Users.Events;
using Application.Channel.ChannelAgencies;
using Application.Spread;

namespace Application.Authorization.Users
{
    public class UserManager : CommonFrameUserManager<Role, User>
    {
        private IRepository<User, long> _userRepository;
        private IRepository<ChannelAgency> _channelAgencyRepository;
        private ChannelAgencyManager _channelAgencyManager;
        private IEventBus EventBus;
        private SpreadManager SpreadManager;

        public UserManager(
            UserStore userStore,
            RoleManager roleManager,
            SpreadManager spreadManager,
            ChannelAgencyManager channelAgencyManager,
            IEventBus eventBus,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<User, long> userRepository,
            IRepository<ChannelAgency> channelAgencyRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IdentityEmailMessageService emailService,
            IUserTokenProviderAccessor userTokenProviderAccessor)
            : base(
                  userStore,
                  roleManager,
                  permissionManager,
                  unitOfWorkManager,
                  cacheManager,
                  organizationUnitRepository,
                  userOrganizationUnitRepository,
                  organizationUnitSettings,
                  localizationManager,
                  emailService,
                  settingManager,
                  userTokenProviderAccessor)
        {
            SpreadManager = spreadManager;
            EventBus = eventBus;
            _channelAgencyRepository = channelAgencyRepository;
            _channelAgencyManager = channelAgencyManager;
            _userRepository = userRepository;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(ApplicationConsts.LocalizationSourceName, name);
        }

        public async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
                {
                    return await FindByIdAsync(userIdentifier.UserId);
                }
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);

            if (user == null)
            {
                throw new ApplicationException(L("ThereIsNoUser") + userIdentifier);
            }
            return user;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        public async Task CheckCanBind(User sourceUser, User parentUser)
        {
            await SpreadManager.CanSpreadAsync(parentUser);

            if (sourceUser.ParentUserId != null)
            {
                throw new InfrastructureException(L("SourceUserHasParent"));
            }

            if (parentUser.ParentUserId == sourceUser.Id)
            {
                throw new InfrastructureException(L("SourceUserIsTargetUserParent"));
            }

            if (sourceUser.Id == parentUser.Id)
            {
                throw new InfrastructureException(L("SourceUserIsTargetUser"));
            }
        }

        [UnitOfWork]
        public async Task BindParent(User sourceUser, User parentUser)
        {
            try
            {
                await CheckCanBind(sourceUser, parentUser);
            }
            catch
            {
                return;
            }

            //source
            sourceUser.ParentUserId = parentUser.Id;

            //channelAgency
            if (parentUser.ChannelAgencyId.HasValue)
            {
                sourceUser.ChannelAgencyId = parentUser.ChannelAgencyId;
                sourceUser.ChannelAgentDepth = parentUser.ChannelAgentDepth + 1;

                _channelAgencyManager.AddNewCustomer(sourceUser.ChannelAgencyId.Value, sourceUser.ChannelAgentDepth);
            }
            else if (parentUser.IsChannelAgency)
            {
                sourceUser.ChannelAgencyId = parentUser.UserChannelAgencyId;
                sourceUser.ChannelAgentDepth = 1;

                _channelAgencyManager.AddNewCustomer(sourceUser.ChannelAgencyId.Value, sourceUser.ChannelAgentDepth);
            }
            _userRepository.Update(sourceUser);

            //
            parentUser.ChildCountOfDepth1 += 1;
            parentUser.GroupCount += 1;
            _userRepository.Update(parentUser);

            //
            if (parentUser.ParentUserId.HasValue)
            {
                parentUser.ParentUser.ChildCountOfDepth2 += 1;
                parentUser.ParentUser.GroupCount += 1;
                _userRepository.Update(parentUser.ParentUser);

                if (parentUser.ParentUser.ParentUserId.HasValue)
                {
                    //
                    parentUser.ParentUser.ParentUser.ChildCountOfDepth3 += 1;
                    parentUser.ParentUser.ParentUser.GroupCount += 1;
                    _userRepository.Update(parentUser.ParentUser.ParentUser);
                }
            }
            EventBus.Trigger(new BindParentEventData(sourceUser, parentUser));
        }

        public async Task BindParent(User sourceUser,long parentUserId)
        {
            User parentUser = _userRepository.Get(parentUserId);
            await BindParent(sourceUser, parentUser);
        }

        [UnitOfWork]
        public User IncreaseSales(User user,decimal money)
        {
            if (money == 0)
            {
                return user;
            }
            user.Sales += money;
            _userRepository.Update(user);
            _unitOfWorkManager.Current.SaveChanges();
            return user;
        }

        public User GetParentUserOfDepth(User sourceUser, int depth)
        {
            if (sourceUser.ParentUserId.HasValue)
            {
                if (depth == 1)
                {
                    return sourceUser.ParentUser;
                }
                else
                {
                    depth--;
                    return GetParentUserOfDepth(sourceUser.ParentUser, depth);
                }
            }
            else
            {
                return null;
            }
        }

        public int GetRankOfUser(long userId)
        {
            User user = _userRepository.Get(userId);
            return GetRankOfUser(user);
        }

        public int GetRankOfUser(User user)
        {
            int rank=_userRepository.GetAll().Where(model => model.IsHide == false&&model.Sales>user.Sales).Count();
            rank++;
            return rank;
        }
    }
}