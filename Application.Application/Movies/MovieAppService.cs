using Application.Movies.Dto;
using Infrastructure.Application.Services;
using Infrastructure.Linq.Extensions;
using Infrastructure.Domain.Repositories;
using System.Linq;
using Application.Members;
using Application.IO;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using Infrastructure.Web.Models;

namespace Application.Movies
{
    public class MovieAppService :
        CrudAppService<
            Movie,
            MovieDto,
            int,
            MovieGetAllInput,
            MovieCreateInput,
            MovieUpdateInput, MovieGetInput, MovieDeleteInput>
        , IMovieAppService
    {
        private MemberCardManager _memberCardManager;

        private IRepository<MovieHint> _movieHintRepository;

        public QiniuFileHelper QiniuFileHelper { get; set; }

        public MovieAppService(IRepository<Movie> repository, IRepository<MovieHint> movieHintRepository, MemberCardManager memberCardManager) : 
            base(repository)
        {
            _memberCardManager = memberCardManager;
            _movieHintRepository = movieHintRepository;
        }

        protected override IQueryable<Movie> CreateFilteredQuery(MovieGetAllInput input)
        {
            return Repository.GetAll().WhereIf(input.MovieCategoryId!=null, model => model.MovieCategoryId == input.MovieCategoryId);
        }

        public virtual MovieForUserDto GetMovieForUser(MovieGetInput input)
        {
            Movie movie = Repository.Get(input.Id);
            IncreaseMovieHint(new MovieHintInput()
            {
                MovieId=input.Id
            });
            return ObjectMapper.Map<MovieForUserDto>(movie);
        }

        public virtual AjaxResponse GetMoviePlayPathForUser(MovieGetInput input)
        {
            Movie movie = Repository.Get(input.Id);
            string path = movie.Path;

            if (movie.MemberLevelId != null|| movie.ShouldBeMemberForPlay)
            {
                if (InfrastructureSession.UserId == null)
                {
                    return new AjaxResponse(new ErrorInfo("you has not login!"),true);
                }
                MemberCard memberCard = _memberCardManager.GetValidMemberCardOfUser(InfrastructureSession.UserId.Value);

                if (memberCard == null)
                {
                    return new AjaxResponse(new ErrorInfo("you has not member!"));
                }

                if (movie.MemberLevelId != null&& memberCard.Level.Id!= movie.MemberLevelId)
                {
                    return new AjaxResponse(new ErrorInfo("you are not !" + movie.MemberLevel.DisplayName));
                }
                path = QiniuFileHelper.GetAuthorizedDownloadPath(movie.Path);
            }
            return new AjaxResponse(path);
        }

        public void IncreaseMovieHint(MovieHintInput input)
        {
            MovieHint movieHint = input.MapTo<MovieHint>();
            movieHint.UserId = InfrastructureSession.UserId;
            _movieHintRepository.Insert(movieHint);
        }

        public PagedResultDto<MovieDto> GetLastestMovies(MovieGetAllInput input)
        {
            input.Sorting = "CreationTime";
            return GetAll(input);
        }

        public PagedResultDto<MovieDto> GetHotMovies(MovieGetAllInput input)
        {
            IQueryable<MovieHint> movieHints = _movieHintRepository.
                GetAll().
                Where(model => model.Movie.MovieCategoryId == input.MovieCategoryId);

            IQueryable<Movie> movieQueryable = from movieHint in movieHints
                                 select movieHint.Movie;

            var totalCount = movieQueryable.Count();

            movieQueryable = ApplySorting(movieQueryable, input);
            movieQueryable = ApplyPaging(movieQueryable, input);

            var movies = movieQueryable.ToList();

            return new PagedResultDto<MovieDto>(
                totalCount,
                input.PageIndex,
                input.PageSize,
                movies.Select(MapToEntityDto).ToList()
            );
        }
    }
}