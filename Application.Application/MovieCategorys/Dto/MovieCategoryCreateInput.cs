using System.ComponentModel.DataAnnotations;
using Infrastructure.Auditing;
using Infrastructure.AutoMapper;

namespace Application.MovieCategorys.Dto
{
    [AutoMap(typeof(MovieCategory))]

    public class MovieCategoryCreateInput
    {
        [Required]
        public string Title { get; set; }
    }
}
