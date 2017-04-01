using System;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.MultiTenancy;

namespace Infrastructure.CommonFrame.Configuration
{
    public class CommonFrameEntityTypes : ICommonFrameEntityTypes
    {
        public Type User
        {
            get { return _user; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof (UserBase).IsAssignableFrom(value))
                {
                    throw new InfrastructureException(value.AssemblyQualifiedName + " should be derived from " + typeof(UserBase).AssemblyQualifiedName);
                }

                _user = value;
            }
        }
        private Type _user;

        public Type Role
        {
            get { return _role; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(RoleBase).IsAssignableFrom(value))
                {
                    throw new InfrastructureException(value.AssemblyQualifiedName + " should be derived from " + typeof(RoleBase).AssemblyQualifiedName);
                }

                _role = value;
            }
        }
        private Type _role;

        public Type Tenant
        {
            get { return _tenant; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(TenantBase).IsAssignableFrom(value))
                {
                    throw new InfrastructureException(value.AssemblyQualifiedName + " should be derived from " + typeof(TenantBase).AssemblyQualifiedName);
                }
                _tenant = value;
            }
        }
        private Type _tenant;
    }
}