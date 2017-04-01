using System;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;

namespace Application.MovieCategorys.Dto
{
    [AutoMapFrom(typeof(MovieCategory))]
    public class MovieCategoryDto:AuditedEntityDto
    {
        public string Title { get; set; }
    }
}
