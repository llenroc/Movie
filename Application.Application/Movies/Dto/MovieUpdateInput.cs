using System.ComponentModel.DataAnnotations;
using Infrastructure.Auditing;
using Infrastructure.AutoMapper;
using Infrastructure.Application.DTO;
using System;
using System.Collections.Generic;

namespace Application.Movies.Dto
{
    [AutoMap(typeof(Movie))]

    public class MovieUpdateInput:EntityDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int MovieCategoryId { get; set; }

        public int MovieType { get; set; }

        public string CoverPath { get; set; }

        public string Poster { get; set; }

        public string Director { get; set; }

        public string Performer { get; set; }

        public string Designation { get; set; }

        public string Description { get; set; }

        public bool IsCode { get; set; }

        public string Country { get; set; }

        public string Path { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool ShouldBeMemberForPlay { get; set; }

        public ICollection<MoviePicture> Pictures { get; set; }
    }
}
