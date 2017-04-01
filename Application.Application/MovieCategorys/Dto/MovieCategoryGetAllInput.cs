using Infrastructure.Application.DTO;

namespace Application.MovieCategorys.Dto
{
    public class MovieCategoryGetAllInput : PagedAndSortedResultRequestDto
    {
        public int Id { get; set; }
    }
}
