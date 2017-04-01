using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wechats.PublicWechats.Dto
{
    public class GeneralSettingsEditDto
    {
        public string Token { get; set; }

        public string AppId { get; set; }

        public string Secret { get; set; }

        public string EncodingAESKey { get; set; }

        [MaxLength(128)]
        public string SubscribeLink { get; set; }
    }
}
