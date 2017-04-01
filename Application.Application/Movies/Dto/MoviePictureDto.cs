using System;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System.Collections.Generic;

namespace Application.Movies.Dto
{
    [AutoMapFrom(typeof(MoviePicture))]
    public class MoviePictureDto:EntityDto
    {
        public string Path { get; set; }
    }
}
