using System.ComponentModel.DataAnnotations;
using Infrastructure.Auditing;
using Infrastructure.AutoMapper;
using Infrastructure.Application.DTO;

namespace Application.MovieCategorys.Dto
{
    [AutoMap(typeof(MovieCategory))]

    public class MovieCategoryUpdateInput:EntityDto
    {
        [Required]
        public string Title { get; set; }
    }
}
