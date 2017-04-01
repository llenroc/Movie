using System.Collections.Generic;

namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserFeatureConfigDto
    {
        public Dictionary<string, StringValueDto> AllFeatures { get; set; }
    }
}