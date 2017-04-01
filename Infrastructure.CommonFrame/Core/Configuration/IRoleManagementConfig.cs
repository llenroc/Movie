using System.Collections.Generic;

namespace Infrastructure.CommonFrame.Configuration
{
    public interface IRoleManagementConfig
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}