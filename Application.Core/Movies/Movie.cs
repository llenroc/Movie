using Application.Members;
using Application.MovieCategorys;
using Infrastructure.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Movies
{
    public class Movie : AuditedEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int MovieCategoryId { get; set; }

        public virtual MovieCategory MovieCategory { get; set; }

        public int MovieType { get; set; }

        public string CoverPath { get; set; }

        public string Poster { get; set; }

        public string Director { get; set; }

        public string Performer { get; set; }

        public string Designation { get; set; }

        public string Description { get; set; }

        public bool IsCode { get; set; }

        public string Country { get; set; }

        [Required]
        public string Path { get; set; }

        public int? MemberLevelId { get; set; }

        public virtual MemberLevel MemberLevel { get;set;}

        public bool IsTop { get; set; }

        public bool IsRecommend { get; set; }

        public bool IsHome { get; set; }

        public bool IsPublish { get; set; }

        public string Author { get; set; }

        public string Tags { get; set; }

        public bool ShouldBeMemberForPlay { get; set; }

        public virtual ICollection<MoviePicture> Pictures { get; set; }

        public Movie()
        {
            IsTop = false;
            IsRecommend = false;
            IsHome = false;
            IsPublish = false;
        }
    }
}
