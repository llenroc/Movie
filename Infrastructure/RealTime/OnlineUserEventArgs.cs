using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RealTime
{
    public class OnlineUserEventArgs : OnlineClientEventArgs
    {
        public UserIdentifier User { get; }

        public OnlineUserEventArgs(UserIdentifier user, IOnlineClient client): base(client)
        {
            User = user;
        }
    }
}
