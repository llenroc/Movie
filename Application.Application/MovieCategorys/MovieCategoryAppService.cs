using Application.MovieCategorys.Dto;
using Infrastructure.Application.Services;
using Infrastructure.Domain.Repositories;

namespace Application.MovieCategorys
{
    public class MovieCategoryAppService:
        CrudAppService<
            MovieCategory,
            MovieCategoryDto,
            int,
            SearchMovieCategoryInput,
            MovieCategoryCreateInput,
            MovieCategoryUpdateInput,MovieCategoryGetInput,MovieCategoryDeleteInput>
        ,IMovieCategoryAppService
    {
        public MovieCategoryAppService(IRepository<MovieCategory> repository) :base(repository)
        {

        }
    }
}
