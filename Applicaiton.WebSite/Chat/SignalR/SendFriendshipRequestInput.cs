using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebSite.Chat.SignalR
{
    public class SendFriendshipRequestInput
    {
        public long UserId { get; set; }

        public int? TenantId { get; set; }
    }
}