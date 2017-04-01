using Application.Wechats.Shares;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wechats.Shares.Dto
{
    [AutoMap(typeof(Share))]
    public class ShareDto:EntityDto
    {
        public string No { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string ImgUrl { get; set; }

        public string Desc { get; set; }

        public int AccessCount { get; set; }
    }
}
