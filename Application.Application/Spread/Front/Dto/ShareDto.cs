﻿using Application.Wechats.Shares;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;

namespace Application.Spread.Front.Dto
{
    [AutoMapFrom(typeof(Share))]
    public class ShareDto:AuditedEntityDto
    {
        public string No { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string ImgUrl { get; set; }

        public string Desc { get; set; }

        public int AccessCount { get; set; }
    }
}
