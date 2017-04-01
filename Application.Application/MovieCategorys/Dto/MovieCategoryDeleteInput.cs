using System;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;

namespace Application.MovieCategorys.Dto
{
    [AutoMapFrom(typeof(MovieCategory))]
    public class MovieCategoryDeleteInput:EntityDto
    {
    }
}
