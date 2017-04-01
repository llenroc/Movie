using Application.Members.MemberCards.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.MemberCards
{
    public class MemberCardAppService : CrudAppService<
        MemberCard,
        MemberCardDto>,
        IMemberCardAppService
    {
        public IMemberCardManager MemberCardManager { get; set; }

        public MemberCardAppService(IRepository<MemberCard> repository) : base(repository)
        {

        }

        public MemberCardDto GetValidMemberCardOfUser()
        {
            long UserId = InfrastructureSession.UserId.Value;
            return MemberCardManager.GetValidMemberCardOfUser(UserId).MapTo<MemberCardDto>();
        }
    }
}
