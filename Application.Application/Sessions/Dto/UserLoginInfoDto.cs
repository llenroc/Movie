﻿using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using Application.Authorization.Users;

namespace Application.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string EmailAddress { get; set; }

        public decimal Sales { get; set; }

        public int GroupCount { get; set; }

        public int ChildCountOfDepth1 { get; set; }

        public int ChildCountOfDepth2 { get; set; }

        public int ChildCountOfDepth3 { get; set; }

        public bool IsSpreader { get; set; }

        public string Avatar { get; set; }
    }
}
