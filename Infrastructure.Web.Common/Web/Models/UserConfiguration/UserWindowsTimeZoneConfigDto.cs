namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserWindowsTimeZoneConfigDto
    {
        public string TimeZoneId { get; set; }

        public double BaseUtcOffsetInMilliseconds { get; set; }

        public double CurrentUtcOffsetInMilliseconds { get; set; }

        public bool IsDaylightSavingTimeNow { get; set; }
    }
}