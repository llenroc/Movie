using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.Services;
using Infrastructure.Application.DTO;
using Application.Movies.Dto;
using Infrastructure.Domain.Repositories;
using Infrastructure.Web.Models;

namespace Application.Movies
{
    public interface IMovieAppService :ICrudAppService<
        MovieDto,
        int, 
        MovieGetAllInput, 
        MovieCreateInput,
        MovieUpdateInput, 
        MovieGetInput,
        MovieDeleteInput>
    {
        MovieForUserDto GetMovieForUser(MovieGetInput input);

        AjaxResponse GetMoviePlayPathForUser(MovieGetInput input);

        PagedResultDto<MovieDto> GetHotMovies(MovieGetAllInput input);

        void IncreaseMovieHint(MovieHintInput input);

        PagedResultDto<MovieDto> GetLastestMovies(MovieGetAllInput input);
    }
}
