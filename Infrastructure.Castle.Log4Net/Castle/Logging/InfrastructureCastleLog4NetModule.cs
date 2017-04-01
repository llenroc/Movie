using Infrastructure.Modules;

namespace Infrastructure.Castle.Logging.Log4Net
{
    /// <summary>
    /// Infrastructure Castle Log4Net module.
    /// </summary>
    [DependsOn(typeof(KernelModule))]
    public class InfrastructureCastleLog4NetModule : InfrastructureModule
    {

    }
}
