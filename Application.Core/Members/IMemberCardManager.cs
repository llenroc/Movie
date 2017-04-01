using Infrastructure.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members
{
    public interface IMemberCardManager : IDomainService
    {
        MemberCard GetValidMemberCardOfUser(long UserId);
        MemberCard CreateMemberCard(MemberCardPackage memberCardPackage, long userId);
    }
}
