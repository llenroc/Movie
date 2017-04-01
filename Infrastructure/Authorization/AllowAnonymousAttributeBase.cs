using System;

namespace Infrastructure.Authorization
{
    /// <summary>
    /// Used to allow a method to be accessed by any user.
    /// Suppress <see cref="InfrastructureAuthorizeAttribute"/> defined in the class containing that method.
    /// </summary>
    public class AllowAnonymousAttributeBase : Attribute, IAllowAnonymousAttribute
    {

    }
}
