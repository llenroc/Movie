namespace Infrastructure.Web.Security.AntiForgery
{
    public interface IInfrastructureAntiForgeryManager
    {
        IAntiForgeryConfiguration Configuration { get; }

        string GenerateToken();
    }
}
