namespace Application.Configuration.Tenant.Dto
{
    public class ShopSettingsEditDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string ShareTitle { get; set; }
        public string ShareDescription { get; set; }
        public string SharePicture { get; set; }
        public string DecreaseStockWhen { get; set; }
        public int OverTimeForDelete { get; set; }
        public bool ShouldHasParentForBuy { get; set; }
        public string DistributionWhen { get; set; }
    }
}
