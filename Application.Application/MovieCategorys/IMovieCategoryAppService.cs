using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Infrastructure.Application.DTO;
using Application.MovieCategorys.Dto;
using Infrastructure.Domain.Repositories;

namespace Application.MovieCategorys
{
    public interface IMovieCategoryAppService :ICrudAppService<MovieCategoryDto, int, SearchMovieCategoryInput, MovieCategoryCreateInput, MovieCategoryUpdateInput, MovieCategoryGetInput, MovieCategoryDeleteInput>
    {
    }
}
