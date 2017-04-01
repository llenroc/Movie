using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Infrastructure.IdentityFramework
{
    public class IdentityResultBase : IdentityResult
    {
        public IdentityResultBase()
        {

        }

        public IdentityResultBase(IEnumerable<string> errors): base(errors)
        {

        }

        public IdentityResultBase(params string[] errors): base(errors)
        {

        }

        public static IdentityResultBase Failed(params string[] errors)
        {
            return new IdentityResultBase(errors);
        }
    }
}
