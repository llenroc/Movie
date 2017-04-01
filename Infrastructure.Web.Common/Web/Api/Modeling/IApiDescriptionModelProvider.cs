namespace Infrastructure.Web.Api.Modeling
{
    public interface IApiDescriptionModelProvider
    {
        ApplicationApiDescriptionModel CreateModel();
    }
}