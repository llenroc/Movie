using System.Collections.Generic;

namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserAuthConfigDto
    {
        public Dictionary<string,string> AllPermissions { get; set; }

        public Dictionary<string, string> GrantedPermissions { get; set; }
        
    }
}