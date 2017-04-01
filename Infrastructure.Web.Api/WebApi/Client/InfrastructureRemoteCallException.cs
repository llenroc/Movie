using System;
using System.Runtime.Serialization;
using Infrastructure.Web.Models;

namespace Infrastructure.WebApi.Client
{
    /// <summary>
    /// This exception is thrown when a remote method call made and remote application sent an error message.
    /// </summary>
    [Serializable]
    public class InfrastructureRemoteCallException : InfrastructureException
    {
        /// <summary>
        /// Remote error information.
        /// </summary>
        public ErrorInfo ErrorInfo { get; set; }

        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        public InfrastructureRemoteCallException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        public InfrastructureRemoteCallException(SerializationInfo serializationInfo, StreamingContext context)  : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="InfrastructureException"/> object.
        /// </summary>
        /// <param name="errorInfo">Exception message</param>
        public InfrastructureRemoteCallException(ErrorInfo errorInfo): base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }
    }
}
