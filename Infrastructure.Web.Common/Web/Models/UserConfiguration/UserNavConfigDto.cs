using System.Collections.Generic;
using Infrastructure.Application.Navigation;

namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserNavConfigDto
    {
        public Dictionary<string, UserMenu> Menus { get; set; }
    }
}