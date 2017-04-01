using System;

namespace Infrastructure.Event.Bus.Exceptions
{
    /// <summary>
    /// This type of events are used to notify for exceptions handled by  infrastructure.
    /// </summary>
    public class HandledExceptionData : ExceptionData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception">Exception object</param>
        public HandledExceptionData(Exception exception): base(exception)
        {

        }
    }
}
