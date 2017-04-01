using Infrastructure.Application.DTO;

namespace Application.Movies.Dto
{
    public class MovieGetAllInput : PagedAndSortedResultRequestDto
    {
        public int? MovieCategoryId { get; set; }
    }
}
