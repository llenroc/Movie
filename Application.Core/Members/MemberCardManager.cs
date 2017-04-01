using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dependency;
using Infrastructure.Domain.Repositories;
using Infrastructure.Timing;
using Infrastructure.Domain.Services;
using Infrastructure;
using Infrastructure.UI;

namespace Application.Members
{
    public class MemberCardManager : DomainService, IMemberCardManager
    {
        public IRepository<MemberCard> Repository { get; set; }

        public MemberCard GetValidMemberCardOfUser(long UserId)
        {
            return Repository.GetAll().Where(
                model => model.UserId == UserId
                &&(model.LimitTime==null
                ||model.LimitTime>Clock.Now)).FirstOrDefault();
        }

        public void CheckUserMemberCard(int memberLevelId, long userId)
        {
            MemberCard userValidMemberCard = GetValidMemberCardOfUser(userId);

            if (userValidMemberCard!=null&& userValidMemberCard.Level.Id == memberLevelId)
            {
                throw new UserFriendlyException("you has been current member");
            }
        }

        public MemberCard CreateMemberCard(MemberCardPackage memberCardPackage,long userId)
        {
            CheckUserMemberCard(memberCardPackage.MemberLevel.Id,userId);
            MemberCard memberCard = new MemberCard()
            {
                Level=memberCardPackage.MemberLevel,
                No= MemberCard.CreateNo(),
                UserId=userId,
                Expiry=memberCardPackage.Expiry,
                LimitTime=Clock.Now.AddDays(memberCardPackage.Expiry)
            };
            Repository.Insert(memberCard);
            return memberCard;
        }
    }
}
