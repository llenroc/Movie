using System;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;

namespace Application.Movies.Dto
{
    [AutoMapFrom(typeof(Movie))]
    public class MovieDeleteInput:EntityDto
    {
    }
}
