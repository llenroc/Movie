using System;
using System.Runtime.Serialization;

namespace Infrastructure
{
    /// <summary>
    /// Base exception type for those are thrown by  system for  specific exceptions.
    /// </summary>
    [Serializable]
    public class InfrastructureException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        public InfrastructureException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        public InfrastructureException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public InfrastructureException(string message): base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public InfrastructureException(string message, Exception innerException)  : base(message, innerException)
        {

        }
    }
}
