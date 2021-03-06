﻿using Application.Authorization.Users;
using Application.MultiTenancy;
using Infrastructure.Application.Services;
using Infrastructure.IdentityFramework;
using Infrastructure.Runtime.Session;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Application
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ApplicationAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected ApplicationAppServiceBase()
        {
            LocalizationSourceName = ApplicationConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            if (InfrastructureSession.UserId != null)
            {
                var user = UserManager.FindByIdAsync(InfrastructureSession.UserId.Value);
                return user;
            }
            else
            {
                return Task.FromResult<User>(null);
            }
           
        }

        protected virtual User GetCurrentUser()
        {
            if (InfrastructureSession.UserId != null)
            {
                var user = UserManager.FindById(InfrastructureSession.UserId.Value);
                return user;
            }
            else
            {
                return null;
            }
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(InfrastructureSession.GetTenantId());
        }

        protected virtual Tenant GetCurrentTenant()
        {
            return TenantManager.GetById(InfrastructureSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}