namespace Infrastructure.Web.Security.AntiForgery
{
    /// <summary>
    /// This interface is internally used by  framework and normally should not be used by applications.
    /// If it's needed, use 
    /// <see cref="IInfrastructureAntiForgeryManager"/> and cast to 
    /// <see cref="IAntiForgeryValidator"/> to use 
    /// <see cref="IsValid"/> method.
    /// </summary>
    public interface IAntiForgeryValidator
    {
        bool IsValid(string cookieValue, string tokenValue);
    }
}
