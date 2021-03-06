﻿using System;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System.Collections.Generic;

namespace Application.Movies.Dto
{
    [AutoMapFrom(typeof(Movie))]
    public class MovieDto:AuditedEntityDto
    {
        public string Title { get; set; }

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

        public bool ShouldBeMemberForPlay { get; set; }

        public IEnumerable<MoviePictureDto> Pictures { get; set; }
    }
}
