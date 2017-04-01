using Application.Wechats.Shares;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wechats.Shares.Dto
{
    [AutoMapTo(typeof(Share))]
    public class ShareForCreateInput
    {
        [Required]
        public string No { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public string ImgUrl { get; set; }

        public string Desc { get; set; }
    }
}
