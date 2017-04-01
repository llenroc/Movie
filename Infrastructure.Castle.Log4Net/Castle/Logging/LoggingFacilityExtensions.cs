using Castle.Facilities.Logging;

namespace Infrastructure.Castle.Logging.Log4Net
{ 
    public static class LoggingFacilityExtensions
    {
        public static LoggingFacility UseInfrastructureLog4Net(this LoggingFacility loggingFacility)
        {
            return loggingFacility.LogUsing<Log4NetLoggerFactory>();
        }
    }
}
