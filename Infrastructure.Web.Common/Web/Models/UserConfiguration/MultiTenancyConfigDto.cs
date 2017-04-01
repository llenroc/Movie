namespace Infrastructure.Web.Models.UserConfiguration
{
    public class MultiTenancyConfigDto
    {
        public bool IsEnabled { get; set; }

        public MultiTenancySidesConfigDto Sides { get; private set; }

        public MultiTenancyConfigDto()
        {
            Sides = new MultiTenancySidesConfigDto();
        }
    }
}