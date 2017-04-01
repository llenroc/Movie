using Application.Authorization.Users;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.Front.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserForRankDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string NickName { get; set; }

        public UserSource Source { get; set; }

        public string Avatar { get; set; }

        public bool IsSpreader { get; set; }

        public decimal Sales { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
