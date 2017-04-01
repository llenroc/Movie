namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserAntiForgeryConfigDto
    {
        public string TokenCookieName { get; set; }

        public string TokenHeaderName { get; set; }
    }
}