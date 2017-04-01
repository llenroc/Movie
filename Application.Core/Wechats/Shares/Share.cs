using Infrastructure.Domain.Entities.Auditing;
using Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Wechats.Shares
{
    public class Share:AuditedEntity
    {
        [Required]
        public string No { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public string ImgUrl { get; set; }

        public string Desc { get; set; }

        public virtual ICollection<ShareAccess> ShareAccesses { get; set; }

        public int GetAccessCount()
        {
            if (ShareAccesses == null)
            {
                return 0;
            }
            return ShareAccesses.Count();
        }

        public static string CreateNo()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
    }
}
