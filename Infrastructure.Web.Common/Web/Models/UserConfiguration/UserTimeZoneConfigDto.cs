namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserTimeZoneConfigDto
    {
        public UserWindowsTimeZoneConfigDto Windows { get; set; }

        public UserIanaTimeZoneConfigDto Iana { get; set; }
    }
}